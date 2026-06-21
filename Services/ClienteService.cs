using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Repositories;

namespace GestaoHigienizePrime.Services;

public class ClienteService : IClienteService
{
    private readonly IGoogleSheetsRepository _repository;

    public ClienteService(IGoogleSheetsRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Cliente>> GetAllAsync()
    {
        var clientes = await _repository.GetClientesAsync();
        return clientes.Where(c => c.Ativo).OrderByDescending(c => c.DataCadastro).ToList();
    }

    public async Task<Cliente?> GetByIdAsync(string id)
    {
        return await _repository.GetClienteByIdAsync(id);
    }

    public async Task<bool> CreateAsync(Cliente cliente)
    {
        cliente.Id = Guid.NewGuid().ToString();
        cliente.DataCadastro = DateTime.Now;
        cliente.Ativo = true;
        return await _repository.InsertClienteAsync(cliente);
    }

    public async Task<bool> UpdateAsync(string id, Cliente cliente)
    {
        cliente.Ativo = true;
        return await _repository.UpdateClienteAsync(id, cliente);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var cliente = await _repository.GetClienteByIdAsync(id);
        if (cliente == null) return false;
        cliente.Ativo = false;
        return await _repository.UpdateClienteAsync(id, cliente);
    }

    public async Task<List<Cliente>> SearchAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return await GetAllAsync();

        var clientes = await _repository.GetClientesAsync();
        termo = termo.ToLowerInvariant();

        return clientes.Where(c =>
            c.Ativo &&
            (c.Nome?.ToLowerInvariant().Contains(termo) == true ||
             c.Telefone?.Contains(termo) == true ||
             c.Cidade?.ToLowerInvariant().Contains(termo) == true))
            .OrderByDescending(c => c.DataCadastro)
            .ToList();
    }
}
