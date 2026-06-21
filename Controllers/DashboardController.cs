using Microsoft.AspNetCore.Mvc;
using GestaoHigienizePrime.Services;

namespace GestaoHigienizePrime.Controllers;

public class DashboardController : Controller
{
    private readonly IRelatorioService _relatorioService;

    public DashboardController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var model = await _relatorioService.GetDashboardDataAsync();
        return View(model);
    }
}
