using Microsoft.AspNetCore.Mvc;
using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Models.Enums;
using GestaoHigienizePrime.Services;
using GestaoHigienizePrime.ViewModels;

namespace GestaoHigienizePrime.Controllers;

public class FinanceiroController : Controller
{
    private readonly IFinanceiroService _financeiroService;

    public FinanceiroController(IFinanceiroService financeiroService)
    {
        _financeiroService = financeiroService;
    }

    public async Task<IActionResult> Index(string tipo = "", int page = 1, int pageSize = 10)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var financeiros = await _financeiroService.GetAllAsync();

        if (!string.IsNullOrEmpty(tipo) && Enum.TryParse<TipoTransacao>(tipo, out var tipoFilter))
            financeiros = financeiros.Where(f => f.Tipo == tipoFilter).ToList();

        var receitaTotal = financeiros.Where(f => f.Tipo == TipoTransacao.Entrada).Sum(f => f.Valor);
        var despesaTotal = financeiros.Where(f => f.Tipo == TipoTransacao.Saida).Sum(f => f.Valor);

        var total = financeiros.Count;
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        var paged = financeiros.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        ViewBag.Tipo = tipo;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalItems = total;
        ViewBag.ReceitaTotal = receitaTotal;
        ViewBag.DespesaTotal = despesaTotal;
        ViewBag.Saldo = receitaTotal - despesaTotal;

        return View(paged);
    }

    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        return View(new FinanceiroViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FinanceiroViewModel model)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
            return View(model);

        var financeiro = new Financeiro
        {
            Tipo = model.Tipo,
            Categoria = model.Categoria,
            Descricao = model.Descricao,
            Valor = model.Valor,
            Data = model.Data,
            ServicoId = model.ServicoId
        };

        var result = await _financeiroService.CreateAsync(financeiro);
        if (!result)
        {
            ModelState.AddModelError("", "Erro ao registrar transação.");
            return View(model);
        }

        TempData["Success"] = "Transação registrada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var financeiro = await _financeiroService.GetByIdAsync(id);
        if (financeiro == null) return NotFound();

        var model = new FinanceiroViewModel
        {
            Id = financeiro.Id,
            Tipo = financeiro.Tipo,
            Categoria = financeiro.Categoria,
            Descricao = financeiro.Descricao,
            Valor = financeiro.Valor,
            Data = financeiro.Data,
            ServicoId = financeiro.ServicoId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, FinanceiroViewModel model)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
            return View(model);

        var financeiro = new Financeiro
        {
            Id = id,
            Tipo = model.Tipo,
            Categoria = model.Categoria,
            Descricao = model.Descricao,
            Valor = model.Valor,
            Data = model.Data,
            ServicoId = model.ServicoId
        };

        var result = await _financeiroService.UpdateAsync(id, financeiro);
        if (!result)
        {
            ModelState.AddModelError("", "Erro ao atualizar transação.");
            return View(model);
        }

        TempData["Success"] = "Transação atualizada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var result = await _financeiroService.DeleteAsync(id);
        if (!result)
        {
            TempData["Error"] = "Erro ao excluir transação.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Transação excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}
