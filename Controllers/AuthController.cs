using Microsoft.AspNetCore.Mvc;
using GestaoHigienizePrime.Services;
using GestaoHigienizePrime.ViewModels;

namespace GestaoHigienizePrime.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("UserId") != null)
            return RedirectToAction("Index", "Dashboard");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var usuario = await _authService.AuthenticateAsync(model.Usuario!, model.Senha!);

        if (usuario == null)
        {
            ModelState.AddModelError("", "Usuário ou senha inválidos.");
            return View(model);
        }

        HttpContext.Session.SetString("UserId", usuario.Id!);
        HttpContext.Session.SetString("UserName", usuario.NomeUsuario!);
        HttpContext.Session.SetString("UserFullName", usuario.NomeCompleto ?? usuario.NomeUsuario!);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
