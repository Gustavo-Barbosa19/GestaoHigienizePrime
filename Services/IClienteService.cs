using GestaoHigienizePrime.Models;

namespace GestaoHigienizePrime.Services;

public interface IClienteService
{
    Task<List<Cliente>> GetAllAsync();
    Task<Cliente?> GetByIdAsync(string id);
    Task<bool> CreateAsync(Cliente cliente);
    Task<bool> UpdateAsync(string id, Cliente cliente);
    Task<bool> DeleteAsync(string id);
    Task<List<Cliente>> SearchAsync(string termo);
}
