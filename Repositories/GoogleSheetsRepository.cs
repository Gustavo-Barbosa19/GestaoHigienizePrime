using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Models.Enums;
using GestaoHigienizePrime.Services;

namespace GestaoHigienizePrime.Repositories;

public class GoogleSheetsRepository : IGoogleSheetsRepository
{
    private readonly IGoogleSheetsService _sheetsService;
    private readonly IConfiguration _configuration;

    public GoogleSheetsRepository(IGoogleSheetsService sheetsService, IConfiguration configuration)
    {
        _sheetsService = sheetsService;
        _configuration = configuration;
    }

    public string PlanilhaUsuarios => _configuration["GoogleSheets:PlanilhaUsuarios"] ?? "Usuarios";
    public string PlanilhaClientes => _configuration["GoogleSheets:PlanilhaClientes"] ?? "Clientes";
    public string PlanilhaServicos => _configuration["GoogleSheets:PlanilhaServicos"] ?? "Servicos";
    public string PlanilhaFinanceiro => _configuration["GoogleSheets:PlanilhaFinanceiro"] ?? "Financeiro";

    public async Task<List<Usuario>> GetUsuariosAsync() => await _sheetsService.GetDataAsync<Usuario>(PlanilhaUsuarios);
    public async Task<Usuario?> GetUsuarioByIdAsync(string id) => await _sheetsService.GetByIdAsync<Usuario>(PlanilhaUsuarios, id);
    public async Task<bool> InsertUsuarioAsync(Usuario usuario) => await _sheetsService.InsertDataAsync(PlanilhaUsuarios, usuario);
    public async Task<bool> UpdateUsuarioAsync(string id, Usuario usuario) => await _sheetsService.UpdateDataAsync(PlanilhaUsuarios, id, usuario);

    public async Task<List<Cliente>> GetClientesAsync() => await _sheetsService.GetDataAsync<Cliente>(PlanilhaClientes);
    public async Task<Cliente?> GetClienteByIdAsync(string id) => await _sheetsService.GetByIdAsync<Cliente>(PlanilhaClientes, id);
    public async Task<bool> InsertClienteAsync(Cliente cliente) => await _sheetsService.InsertDataAsync(PlanilhaClientes, cliente);
    public async Task<bool> UpdateClienteAsync(string id, Cliente cliente) => await _sheetsService.UpdateDataAsync(PlanilhaClientes, id, cliente);
    public async Task<bool> DeleteClienteAsync(string id) => await _sheetsService.DeleteDataAsync(PlanilhaClientes, id);
    public async Task<List<Cliente>> SearchClientesAsync(string termo) => await _sheetsService.QueryDataAsync<Cliente>(PlanilhaClientes, "nome", termo);

    public async Task<List<Servico>> GetServicosAsync() => await _sheetsService.GetDataAsync<Servico>(PlanilhaServicos);
    public async Task<Servico?> GetServicoByIdAsync(string id) => await _sheetsService.GetByIdAsync<Servico>(PlanilhaServicos, id);
    public async Task<bool> InsertServicoAsync(Servico servico) => await _sheetsService.InsertDataAsync(PlanilhaServicos, servico);
    public async Task<bool> UpdateServicoAsync(string id, Servico servico) => await _sheetsService.UpdateDataAsync(PlanilhaServicos, id, servico);
    public async Task<bool> DeleteServicoAsync(string id) => await _sheetsService.DeleteDataAsync(PlanilhaServicos, id);
    public async Task<List<Servico>> SearchServicosAsync(string termo) => await _sheetsService.QueryDataAsync<Servico>(PlanilhaServicos, "clienteNome", termo);

    public async Task<List<Financeiro>> GetFinanceiroAsync() => await _sheetsService.GetDataAsync<Financeiro>(PlanilhaFinanceiro);
    public async Task<Financeiro?> GetFinanceiroByIdAsync(string id) => await _sheetsService.GetByIdAsync<Financeiro>(PlanilhaFinanceiro, id);
    public async Task<bool> InsertFinanceiroAsync(Financeiro financeiro) => await _sheetsService.InsertDataAsync(PlanilhaFinanceiro, financeiro);
    public async Task<bool> UpdateFinanceiroAsync(string id, Financeiro financeiro) => await _sheetsService.UpdateDataAsync(PlanilhaFinanceiro, id, financeiro);
    public async Task<bool> DeleteFinanceiroAsync(string id) => await _sheetsService.DeleteDataAsync(PlanilhaFinanceiro, id);
}
