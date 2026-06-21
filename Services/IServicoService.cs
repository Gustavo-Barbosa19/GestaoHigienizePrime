using GestaoHigienizePrime.Models;

namespace GestaoHigienizePrime.Services;

public interface IServicoService
{
    Task<List<Servico>> GetAllAsync();
    Task<Servico?> GetByIdAsync(string id);
    Task<bool> CreateAsync(Servico servico);
    Task<bool> UpdateAsync(string id, Servico servico);
    Task<bool> DeleteAsync(string id);
    Task<List<Servico>> GetByPeriodoAsync(DateTime inicio, DateTime fim);
    Task<List<Servico>> GetByClienteAsync(string clienteId);
    Task<List<Servico>> GetByStatusAsync(string status);
    Task<Dictionary<string, decimal>> GetFaturamentoMensalAsync(int ano);
}
