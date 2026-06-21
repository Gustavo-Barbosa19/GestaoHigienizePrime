using GestaoHigienizePrime.Models;

namespace GestaoHigienizePrime.Services;

public interface IRelatorioService
{
    Task<DashboardViewModel> GetDashboardDataAsync();
    Task<List<Servico>> GetServicosPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<List<Servico>> GetServicosPorClienteAsync(string clienteId);
    Task<byte[]> ExportarRelatorioPdfAsync(string tipo, DateTime? inicio, DateTime? fim, string? clienteId);
    Task<byte[]> ExportarRelatorioExcelAsync(string tipo, DateTime? inicio, DateTime? fim, string? clienteId);
}
