using Microsoft.AspNetCore.Mvc;
using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Services;

namespace GestaoHigienizePrime.Controllers;

public class TipoServicosController : Controller
{
    private readonly ITipoServicoService _tipoServicoService;

    public TipoServicosController(ITipoServicoService tipoServicoService)
    {
        _tipoServicoService = tipoServicoService;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var tipos = await _tipoServicoService.GetAllAsync();
        return View(tipos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string nome, string descricao)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        if (string.IsNullOrWhiteSpace(nome))
        {
            TempData["Error"] = "Nome do serviço é obrigatório.";
            return RedirectToAction(nameof(Index));
        }

        var tipo = new TipoServico { Nome = nome, Descricao = descricao };
        var result = await _tipoServicoService.CreateAsync(tipo);

        TempData[result ? "Success" : "Error"] = result
            ? "Tipo de serviço cadastrado!"
            : "Erro ao cadastrar tipo de serviço.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var result = await _tipoServicoService.DeleteAsync(id);
        TempData[result ? "Success" : "Error"] = result
            ? "Tipo de serviço excluído!"
            : "Erro ao excluir.";

        return RedirectToAction(nameof(Index));
    }
}
