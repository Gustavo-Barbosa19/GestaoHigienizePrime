using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Services;

namespace GestaoHigienizePrime.Services;

public class TipoServicoService : ITipoServicoService
{
    private readonly IGoogleSheetsService _sheetsService;
    private const string SheetName = "TipoServicos";

    private static readonly List<TipoServico> DefaultServicos = new()
    {
        new() { Id = "1", Nome = "Higienização de Sofá", Ativo = true },
        new() { Id = "2", Nome = "Higienização de Colchão", Ativo = true },
        new() { Id = "3", Nome = "Higienização de Poltrona", Ativo = true },
        new() { Id = "4", Nome = "Higienização de Cadeiras", Ativo = true },
        new() { Id = "5", Nome = "Impermeabilização", Ativo = true },
        new() { Id = "6", Nome = "Limpeza de Tapetes", Ativo = true },
        new() { Id = "7", Nome = "Limpeza de Carpetes", Ativo = true }
    };

    public TipoServicoService(IGoogleSheetsService sheetsService)
    {
        _sheetsService = sheetsService;
    }

    public async Task<List<TipoServico>> GetAllAsync()
    {
        try
        {
            var tipos = await _sheetsService.GetDataAsync<TipoServico>(SheetName);
            if (tipos == null || tipos.Count == 0)
            {
                foreach (var servico in DefaultServicos)
                {
                    await _sheetsService.InsertDataAsync(SheetName, servico);
                }
                return DefaultServicos;
            }
            return tipos.Where(t => t.Ativo).ToList();
        }
        catch
        {
            return DefaultServicos;
        }
    }

    public async Task<bool> CreateAsync(TipoServico tipoServico)
    {
        tipoServico.Id = Guid.NewGuid().ToString();
        tipoServico.Ativo = true;
        return await _sheetsService.InsertDataAsync(SheetName, tipoServico);
    }

    public async Task<bool> UpdateAsync(string id, TipoServico tipoServico)
    {
        return await _sheetsService.UpdateDataAsync(SheetName, id, tipoServico);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var tipo = (await _sheetsService.GetDataAsync<TipoServico>(SheetName)).FirstOrDefault(t => t.Id == id);
        if (tipo == null) return false;
        tipo.Ativo = false;
        return await _sheetsService.UpdateDataAsync(SheetName, id, tipo);
    }
}
