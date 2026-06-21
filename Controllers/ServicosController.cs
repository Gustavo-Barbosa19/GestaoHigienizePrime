using Microsoft.AspNetCore.Mvc;
using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Models.Enums;
using GestaoHigienizePrime.Services;
using GestaoHigienizePrime.ViewModels;

namespace GestaoHigienizePrime.Controllers;

public class ServicosController : Controller
{
    private readonly IServicoService _servicoService;
    private readonly IClienteService _clienteService;
    private readonly ITipoServicoService _tipoServicoService;

    public ServicosController(
        IServicoService servicoService,
        IClienteService clienteService,
        ITipoServicoService tipoServicoService)
    {
        _servicoService = servicoService;
        _clienteService = clienteService;
        _tipoServicoService = tipoServicoService;
    }

    public async Task<IActionResult> Index(string statusFilter = "", int page = 1, int pageSize = 10)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var servicos = await _servicoService.GetAllAsync();

        if (!string.IsNullOrEmpty(statusFilter))
            servicos = servicos.Where(s => s.Status.ToString() == statusFilter).ToList();

        var total = servicos.Count;
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        var pagedServicos = servicos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        ViewBag.StatusFilter = statusFilter;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalItems = total;
        ViewBag.StatusList = Enum.GetValues<StatusServico>();

        return View(pagedServicos);
    }

    public async Task<IActionResult> Details(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var servico = await _servicoService.GetByIdAsync(id);
        if (servico == null) return NotFound();

        return View(servico);
    }

    public async Task<IActionResult> Create()
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var model = new ServicoViewModel
        {
            Clientes = await _clienteService.GetAllAsync(),
            TiposServico = await _tipoServicoService.GetAllAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServicoViewModel model)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
        {
            model.Clientes = await _clienteService.GetAllAsync();
            model.TiposServico = await _tipoServicoService.GetAllAsync();
            return View(model);
        }

        var servico = new Servico
        {
            ClienteId = model.ClienteId,
            DataAtendimento = model.DataAtendimento,
            Horario = model.Horario,
            TipoServico = model.TipoServico,
            QuantidadeItens = model.QuantidadeItens,
            Valor = model.Valor,
            FormaPagamento = model.FormaPagamento,
            Status = model.Status,
            Observacoes = model.Observacoes
        };

        var result = await _servicoService.CreateAsync(servico);
        if (!result)
        {
            ModelState.AddModelError("", "Erro ao criar ordem de serviço.");
            model.Clientes = await _clienteService.GetAllAsync();
            model.TiposServico = await _tipoServicoService.GetAllAsync();
            return View(model);
        }

        TempData["Success"] = "Ordem de serviço criada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var servico = await _servicoService.GetByIdAsync(id);
        if (servico == null) return NotFound();

        var model = new ServicoViewModel
        {
            Id = servico.Id,
            ClienteId = servico.ClienteId,
            DataAtendimento = servico.DataAtendimento,
            Horario = servico.Horario,
            TipoServico = servico.TipoServico,
            QuantidadeItens = servico.QuantidadeItens,
            Valor = servico.Valor,
            FormaPagamento = servico.FormaPagamento,
            Status = servico.Status,
            Observacoes = servico.Observacoes,
            Clientes = await _clienteService.GetAllAsync(),
            TiposServico = await _tipoServicoService.GetAllAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ServicoViewModel model)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
        {
            model.Clientes = await _clienteService.GetAllAsync();
            model.TiposServico = await _tipoServicoService.GetAllAsync();
            return View(model);
        }

        var servico = new Servico
        {
            Id = id,
            ClienteId = model.ClienteId,
            DataAtendimento = model.DataAtendimento,
            Horario = model.Horario,
            TipoServico = model.TipoServico,
            QuantidadeItens = model.QuantidadeItens,
            Valor = model.Valor,
            FormaPagamento = model.FormaPagamento,
            Status = model.Status,
            Observacoes = model.Observacoes
        };

        var result = await _servicoService.UpdateAsync(id, servico);
        if (!result)
        {
            ModelState.AddModelError("", "Erro ao atualizar ordem de serviço.");
            model.Clientes = await _clienteService.GetAllAsync();
            model.TiposServico = await _tipoServicoService.GetAllAsync();
            return View(model);
        }

        TempData["Success"] = "Ordem de serviço atualizada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToAction("Login", "Auth");

        var result = await _servicoService.DeleteAsync(id);
        if (!result)
        {
            TempData["Error"] = "Erro ao excluir ordem de serviço.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Ordem de serviço excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}
