using Microsoft.AspNetCore.Mvc;

namespace GestaoHigienizePrime.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Dashboard");
    }

    public IActionResult Error()
    {
        return View();
    }
}
