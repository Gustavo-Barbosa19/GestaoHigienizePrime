using GestaoHigienizePrime.Models;

namespace GestaoHigienizePrime.Services;

public interface IFinanceiroService
{
    Task<List<Financeiro>> GetAllAsync();
    Task<Financeiro?> GetByIdAsync(string id);
    Task<bool> CreateAsync(Financeiro financeiro);
    Task<bool> UpdateAsync(string id, Financeiro financeiro);
    Task<bool> DeleteAsync(string id);
    Task<List<Financeiro>> GetByPeriodoAsync(DateTime inicio, DateTime fim);
    Task<decimal> GetTotalReceitasAsync();
    Task<decimal> GetTotalDespesasAsync();
    Task<decimal> GetReceitasMesAsync(int mes, int ano);
    Task<decimal> GetDespesasMesAsync(int mes, int ano);
}
