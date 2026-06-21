using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Models.Enums;
using GestaoHigienizePrime.Repositories;

namespace GestaoHigienizePrime.Services;

public class ServicoService : IServicoService
{
    private readonly IGoogleSheetsRepository _repository;
    private readonly IClienteService _clienteService;

    public ServicoService(IGoogleSheetsRepository repository, IClienteService clienteService)
    {
        _repository = repository;
        _clienteService = clienteService;
    }

    public async Task<List<Servico>> GetAllAsync()
    {
        var servicos = await _repository.GetServicosAsync();
        return servicos.OrderByDescending(s => s.DataAtendimento).ToList();
    }

    public async Task<Servico?> GetByIdAsync(string id)
    {
        return await _repository.GetServicoByIdAsync(id);
    }

    public async Task<bool> CreateAsync(Servico servico)
    {
        servico.Id = Guid.NewGuid().ToString();
        servico.DataCriacao = DateTime.Now;

        var cliente = await _clienteService.GetByIdAsync(servico.ClienteId!);
        servico.ClienteNome = cliente?.Nome;

        var created = await _repository.InsertServicoAsync(servico);

        if (created && servico.Status == StatusServico.Finalizado)
        {
            await RegistrarFinanceiroAsync(servico);
        }

        return created;
    }

    public async Task<bool> UpdateAsync(string id, Servico servico)
    {
        var existing = await GetByIdAsync(id);
        if (existing == null) return false;

        servico.DataCriacao = existing.DataCriacao;

        var cliente = await _clienteService.GetByIdAsync(servico.ClienteId!);
        servico.ClienteNome = cliente?.Nome;

        var updated = await _repository.UpdateServicoAsync(id, servico);

        if (updated && servico.Status == StatusServico.Finalizado && existing.Status != StatusServico.Finalizado)
        {
            await RegistrarFinanceiroAsync(servico);
        }

        return updated;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _repository.DeleteServicoAsync(id);
    }

    public async Task<List<Servico>> GetByPeriodoAsync(DateTime inicio, DateTime fim)
    {
        var servicos = await GetAllAsync();
        return servicos.Where(s => s.DataAtendimento.Date >= inicio.Date && s.DataAtendimento.Date <= fim.Date).ToList();
    }

    public async Task<List<Servico>> GetByClienteAsync(string clienteId)
    {
        var servicos = await GetAllAsync();
        return servicos.Where(s => s.ClienteId == clienteId).ToList();
    }

    public async Task<List<Servico>> GetByStatusAsync(string status)
    {
        var servicos = await GetAllAsync();
        return servicos.Where(s => s.Status.ToString() == status).ToList();
    }

    public async Task<Dictionary<string, decimal>> GetFaturamentoMensalAsync(int ano)
    {
        var servicos = await GetAllAsync();
        var finalizados = servicos.Where(s =>
            s.Status == StatusServico.Finalizado &&
            s.DataAtendimento.Year == ano);

        return finalizados
            .GroupBy(s => s.DataAtendimento.ToString("MM"))
            .ToDictionary(g => g.Key, g => g.Sum(s => s.Valor));
    }

    private async Task RegistrarFinanceiroAsync(Servico servico)
    {
        var financeiroRepo = _repository;
        var financeiro = new Financeiro
        {
            Id = Guid.NewGuid().ToString(),
            Tipo = TipoTransacao.Entrada,
            Categoria = "Serviços Realizados",
            Descricao = $"Serviço: {servico.TipoServico} - {servico.ClienteNome}",
            Valor = servico.Valor,
            Data = servico.DataAtendimento,
            ServicoId = servico.Id,
            DataCriacao = DateTime.Now
        };

        await financeiroRepo.InsertFinanceiroAsync(financeiro);
    }
}
