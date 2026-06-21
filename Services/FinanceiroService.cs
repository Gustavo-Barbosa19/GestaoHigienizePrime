using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Models.Enums;
using GestaoHigienizePrime.Repositories;

namespace GestaoHigienizePrime.Services;

public class FinanceiroService : IFinanceiroService
{
    private readonly IGoogleSheetsRepository _repository;

    public FinanceiroService(IGoogleSheetsRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Financeiro>> GetAllAsync()
    {
        return (await _repository.GetFinanceiroAsync())
            .OrderByDescending(f => f.Data)
            .ToList();
    }

    public async Task<Financeiro?> GetByIdAsync(string id)
    {
        return await _repository.GetFinanceiroByIdAsync(id);
    }

    public async Task<bool> CreateAsync(Financeiro financeiro)
    {
        financeiro.Id = Guid.NewGuid().ToString();
        financeiro.DataCriacao = DateTime.Now;
        return await _repository.InsertFinanceiroAsync(financeiro);
    }

    public async Task<bool> UpdateAsync(string id, Financeiro financeiro)
    {
        return await _repository.UpdateFinanceiroAsync(id, financeiro);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _repository.DeleteFinanceiroAsync(id);
    }

    public async Task<List<Financeiro>> GetByPeriodoAsync(DateTime inicio, DateTime fim)
    {
        var financeiros = await GetAllAsync();
        return financeiros.Where(f => f.Data.Date >= inicio.Date && f.Data.Date <= fim.Date).ToList();
    }

    public async Task<decimal> GetTotalReceitasAsync()
    {
        var financeiros = await GetAllAsync();
        return financeiros.Where(f => f.Tipo == TipoTransacao.Entrada).Sum(f => f.Valor);
    }

    public async Task<decimal> GetTotalDespesasAsync()
    {
        var financeiros = await GetAllAsync();
        return financeiros.Where(f => f.Tipo == TipoTransacao.Saida).Sum(f => f.Valor);
    }

    public async Task<decimal> GetReceitasMesAsync(int mes, int ano)
    {
        var financeiros = await GetAllAsync();
        return financeiros.Where(f =>
            f.Tipo == TipoTransacao.Entrada &&
            f.Data.Month == mes &&
            f.Data.Year == ano).Sum(f => f.Valor);
    }

    public async Task<decimal> GetDespesasMesAsync(int mes, int ano)
    {
        var financeiros = await GetAllAsync();
        return financeiros.Where(f =>
            f.Tipo == TipoTransacao.Saida &&
            f.Data.Month == mes &&
            f.Data.Year == ano).Sum(f => f.Valor);
    }
}
