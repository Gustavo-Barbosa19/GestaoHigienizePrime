namespace GestaoHigienizePrime.Models;

public class DashboardViewModel
{
    public int TotalClientes { get; set; }
    public int TotalServicos { get; set; }
    public int ServicosAgendados { get; set; }
    public int ServicosFinalizados { get; set; }
    public decimal FaturamentoMes { get; set; }
    public decimal FaturamentoAnual { get; set; }
    public List<Servico> UltimosAtendimentos { get; set; } = new();
    public List<Servico> ServicosPorMes { get; set; } = new();
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal LucroLiquido { get; set; }
    public decimal ReceitaMensal { get; set; }
    public decimal DespesaMensal { get; set; }
}
