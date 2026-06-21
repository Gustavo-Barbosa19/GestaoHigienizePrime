using Microsoft.AspNetCore.Mvc;
using GestaoHigienizePrime.Services;
using GestaoHigienizePrime.ViewModels;

namespace GestaoHigienizePrime.Controllers;

public class RelatoriosController : Controller
{
    private readonly IRelatorioService _relatorioService;
    private readonly IClienteService _clienteService;

    public RelatoriosController(IRelatorioService relatorioService, IClienteService clienteService)
    {
        _relatorioService = relatorioService;
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        ViewBag.Clientes = await _clienteService.GetAllAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Filtrar(RelatorioViewModel model)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var now = DateTime.Now;
        model.DataInicio = model.Periodo switch
        {
            "dia" => now.Date,
            "semana" => now.Date.AddDays(-7),
            "mes" => new DateTime(now.Year, now.Month, 1),
            "ano" => new DateTime(now.Year, 1, 1),
            _ => model.DataInicio ?? now.Date.AddMonths(-1)
        };
        model.DataFim ??= now;

        var servicos = await _relatorioService.GetServicosPorPeriodoAsync(model.DataInicio.Value, model.DataFim.Value);

        if (!string.IsNullOrEmpty(model.ClienteId))
            servicos = servicos.Where(s => s.ClienteId == model.ClienteId).ToList();

        model.Servicos = servicos;
        model.TotalReceitas = servicos.Sum(s => s.Valor);

        ViewBag.Clientes = await _clienteService.GetAllAsync();
        return View("Index", model);
    }

    public async Task<IActionResult> ExportarPdf(string tipo, DateTime? inicio, DateTime? fim, string? clienteId)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var bytes = await _relatorioService.ExportarRelatorioPdfAsync(tipo, inicio, fim, clienteId);
        return File(bytes, "application/pdf", $"relatorio_higienizeprime.pdf");
    }

    public async Task<IActionResult> ExportarExcel(string tipo, DateTime? inicio, DateTime? fim, string? clienteId)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var bytes = await _relatorioService.ExportarRelatorioExcelAsync(tipo, inicio, fim, clienteId);
        return File(bytes, "text/csv", $"relatorio_higienizeprime.csv");
    }
}
