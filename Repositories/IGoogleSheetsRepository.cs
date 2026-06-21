using GestaoHigienizePrime.Models;

namespace GestaoHigienizePrime.Repositories;

public interface IGoogleSheetsRepository
{
    string PlanilhaUsuarios { get; }
    string PlanilhaClientes { get; }
    string PlanilhaServicos { get; }
    string PlanilhaFinanceiro { get; }

    Task<List<Usuario>> GetUsuariosAsync();
    Task<Usuario?> GetUsuarioByIdAsync(string id);
    Task<bool> InsertUsuarioAsync(Usuario usuario);
    Task<bool> UpdateUsuarioAsync(string id, Usuario usuario);

    Task<List<Cliente>> GetClientesAsync();
    Task<Cliente?> GetClienteByIdAsync(string id);
    Task<bool> InsertClienteAsync(Cliente cliente);
    Task<bool> UpdateClienteAsync(string id, Cliente cliente);
    Task<bool> DeleteClienteAsync(string id);
    Task<List<Cliente>> SearchClientesAsync(string termo);

    Task<List<Servico>> GetServicosAsync();
    Task<Servico?> GetServicoByIdAsync(string id);
    Task<bool> InsertServicoAsync(Servico servico);
    Task<bool> UpdateServicoAsync(string id, Servico servico);
    Task<bool> DeleteServicoAsync(string id);
    Task<List<Servico>> SearchServicosAsync(string termo);

    Task<List<Financeiro>> GetFinanceiroAsync();
    Task<Financeiro?> GetFinanceiroByIdAsync(string id);
    Task<bool> InsertFinanceiroAsync(Financeiro financeiro);
    Task<bool> UpdateFinanceiroAsync(string id, Financeiro financeiro);
    Task<bool> DeleteFinanceiroAsync(string id);
}
