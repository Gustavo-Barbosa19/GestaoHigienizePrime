using Microsoft.AspNetCore.Mvc;
using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Services;
using GestaoHigienizePrime.ViewModels;

namespace GestaoHigienizePrime.Controllers;

public class ClientesController : Controller
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index(string searchTerm = "", int page = 1, int pageSize = 10)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var clientes = string.IsNullOrEmpty(searchTerm)
            ? await _clienteService.GetAllAsync()
            : await _clienteService.SearchAsync(searchTerm);

        var total = clientes.Count;
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        var pagedClientes = clientes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalItems = total;

        return View(pagedClientes);
    }

    public async Task<IActionResult> Details(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var cliente = await _clienteService.GetByIdAsync(id);
        if (cliente == null) return NotFound();

        return View(cliente);
    }

    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteViewModel model)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
            return View(model);

        var cliente = new Cliente
        {
            Nome = model.Nome,
            Telefone = model.Telefone,
            WhatsApp = model.WhatsApp,
            Email = model.Email,
            CEP = model.CEP,
            Endereco = model.Endereco,
            Numero = model.Numero,
            Bairro = model.Bairro,
            Cidade = model.Cidade,
            Estado = model.Estado,
            Observacoes = model.Observacoes
        };

        var result = await _clienteService.CreateAsync(cliente);
        if (!result)
        {
            ModelState.AddModelError("", "Erro ao cadastrar cliente. Tente novamente.");
            return View(model);
        }

        TempData["Success"] = "Cliente cadastrado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var cliente = await _clienteService.GetByIdAsync(id);
        if (cliente == null) return NotFound();

        var model = new ClienteViewModel
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Telefone = cliente.Telefone,
            WhatsApp = cliente.WhatsApp,
            Email = cliente.Email,
            CEP = cliente.CEP,
            Endereco = cliente.Endereco,
            Numero = cliente.Numero,
            Bairro = cliente.Bairro,
            Cidade = cliente.Cidade,
            Estado = cliente.Estado,
            Observacoes = cliente.Observacoes
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ClienteViewModel model)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
            return View(model);

        var cliente = new Cliente
        {
            Id = id,
            Nome = model.Nome,
            Telefone = model.Telefone,
            WhatsApp = model.WhatsApp,
            Email = model.Email,
            CEP = model.CEP,
            Endereco = model.Endereco,
            Numero = model.Numero,
            Bairro = model.Bairro,
            Cidade = model.Cidade,
            Estado = model.Estado,
            Observacoes = model.Observacoes
        };

        var result = await _clienteService.UpdateAsync(id, cliente);
        if (!result)
        {
            ModelState.AddModelError("", "Erro ao atualizar cliente. Tente novamente.");
            return View(model);
        }

        TempData["Success"] = "Cliente atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var result = await _clienteService.DeleteAsync(id);
        if (!result)
        {
            TempData["Error"] = "Erro ao excluir cliente.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Cliente excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}
