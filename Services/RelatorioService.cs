using System.Text;
using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Models.Enums;

namespace GestaoHigienizePrime.Services;

public class RelatorioService : IRelatorioService
{
    private readonly IServicoService _servicoService;
    private readonly IClienteService _clienteService;
    private readonly IFinanceiroService _financeiroService;

    public RelatorioService(
        IServicoService servicoService,
        IClienteService clienteService,
        IFinanceiroService financeiroService)
    {
        _servicoService = servicoService;
        _clienteService = clienteService;
        _financeiroService = financeiroService;
    }

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var clientes = await _clienteService.GetAllAsync();
        var servicos = await _servicoService.GetAllAsync();
        var financeiro = await _financeiroService.GetAllAsync();
        var now = DateTime.Now;

        var model = new DashboardViewModel
        {
            TotalClientes = clientes.Count,
            TotalServicos = servicos.Count,
            ServicosAgendados = servicos.Count(s => s.Status == StatusServico.Agendado),
            ServicosFinalizados = servicos.Count(s => s.Status == StatusServico.Finalizado),
            FaturamentoMes = servicos
                .Where(s => s.Status == StatusServico.Finalizado &&
                           s.DataAtendimento.Month == now.Month &&
                           s.DataAtendimento.Year == now.Year)
                .Sum(s => s.Valor),
            FaturamentoAnual = servicos
                .Where(s => s.Status == StatusServico.Finalizado &&
                           s.DataAtendimento.Year == now.Year)
                .Sum(s => s.Valor),
            UltimosAtendimentos = servicos
                .Where(s => s.Status == StatusServico.Finalizado)
                .OrderByDescending(s => s.DataAtendimento)
                .Take(10)
                .ToList(),
            TotalReceitas = financeiro.Where(f => f.Tipo == TipoTransacao.Entrada).Sum(f => f.Valor),
            TotalDespesas = financeiro.Where(f => f.Tipo == TipoTransacao.Saida).Sum(f => f.Valor),
            ReceitaMensal = financeiro
                .Where(f => f.Tipo == TipoTransacao.Entrada &&
                           f.Data.Month == now.Month &&
                           f.Data.Year == now.Year)
                .Sum(f => f.Valor),
            DespesaMensal = financeiro
                .Where(f => f.Tipo == TipoTransacao.Saida &&
                           f.Data.Month == now.Month &&
                           f.Data.Year == now.Year)
                .Sum(f => f.Valor)
        };

        model.LucroLiquido = model.TotalReceitas - model.TotalDespesas;

        return model;
    }

    public async Task<List<Servico>> GetServicosPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _servicoService.GetByPeriodoAsync(inicio, fim);
    }

    public async Task<List<Servico>> GetServicosPorClienteAsync(string clienteId)
    {
        return await _servicoService.GetByClienteAsync(clienteId);
    }

    public async Task<byte[]> ExportarRelatorioPdfAsync(string tipo, DateTime? inicio, DateTime? fim, string? clienteId)
    {
        var html = new StringBuilder();
        html.AppendLine("<html><head><meta charset='utf-8'>");
        html.AppendLine("<style>body{font-family:Arial;padding:20px}");
        html.AppendLine("h1{color:#1a237e;text-align:center}");
        html.AppendLine("table{width:100%;border-collapse:collapse;margin-top:20px}");
        html.AppendLine("th,td{border:1px solid #ddd;padding:8px;text-align:left}");
        html.AppendLine("th{background-color:#1a237e;color:white}");
        html.AppendLine(".header{text-align:center;margin-bottom:30px}");
        html.AppendLine("</style></head><body>");
        html.AppendLine("<div class='header'><h1>Higienize Prime</h1>");
        html.AppendLine($"<p>Relatório gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}</p></div>");

        if (tipo == "servicos" || tipo == "todos")
        {
            var servicos = clienteId != null
                ? await GetServicosPorClienteAsync(clienteId)
                : await GetServicosPorPeriodoAsync(inicio ?? DateTime.MinValue, fim ?? DateTime.MaxValue);

            html.AppendLine("<h2>Relatório de Serviços</h2>");
            html.AppendLine("<table><thead><tr>");
            html.AppendLine("<th>Cliente</th><th>Data</th><th>Tipo</th><th>Valor</th><th>Status</th>");
            html.AppendLine("</tr></thead><tbody>");

            foreach (var s in servicos)
            {
                html.AppendLine($"<tr><td>{s.ClienteNome}</td><td>{s.DataAtendimento:dd/MM/yyyy}</td>");
                html.AppendLine($"<td>{s.TipoServico}</td><td>R$ {s.Valor:F2}</td><td>{s.Status}</td></tr>");
            }

            html.AppendLine("</tbody></table>");
            html.AppendLine($"<p><strong>Total: R$ {servicos.Sum(s => s.Valor):F2}</strong></p>");
        }

        html.AppendLine("</body></html>");
        return Encoding.UTF8.GetBytes(html.ToString());
    }

    public async Task<byte[]> ExportarRelatorioExcelAsync(string tipo, DateTime? inicio, DateTime? fim, string? clienteId)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Relatório Higienize Prime");
        csv.AppendLine($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}");
        csv.AppendLine();

        if (tipo == "servicos" || tipo == "todos")
        {
            var servicos = clienteId != null
                ? await GetServicosPorClienteAsync(clienteId)
                : await GetServicosPorPeriodoAsync(inicio ?? DateTime.MinValue, fim ?? DateTime.MaxValue);

            csv.AppendLine("SERVIÇOS");
            csv.AppendLine("Cliente;Data;Tipo;Valor;Status");

            foreach (var s in servicos)
            {
                csv.AppendLine($"{s.ClienteNome};{s.DataAtendimento:dd/MM/yyyy};{s.TipoServico};{s.Valor:F2};{s.Status}");
            }

            csv.AppendLine($";;;Total;{servicos.Sum(s => s.Valor):F2}");
        }

        return Encoding.UTF8.GetBytes(csv.ToString());
    }
}
