using GestaoHigienizePrime.Models;

namespace GestaoHigienizePrime.Services;

public interface ITipoServicoService
{
    Task<List<TipoServico>> GetAllAsync();
    Task<bool> CreateAsync(TipoServico tipoServico);
    Task<bool> UpdateAsync(string id, TipoServico tipoServico);
    Task<bool> DeleteAsync(string id);
}
