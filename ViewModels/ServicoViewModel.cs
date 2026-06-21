using System.ComponentModel.DataAnnotations;
using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Models.Enums;

namespace GestaoHigienizePrime.ViewModels;

public class ServicoViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Cliente é obrigatório")]
    public string? ClienteId { get; set; }

    public string? ClienteNome { get; set; }

    [Required(ErrorMessage = "Data é obrigatória")]
    public DateTime DataAtendimento { get; set; } = DateTime.Now;

    public string? Horario { get; set; }

    [Required(ErrorMessage = "Tipo de serviço é obrigatório")]
    public string? TipoServico { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
    public int QuantidadeItens { get; set; } = 1;

    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    public string? FormaPagamento { get; set; }
    public StatusServico Status { get; set; } = StatusServico.Agendado;
    public string? Observacoes { get; set; }
    public List<Cliente>? Clientes { get; set; }
    public List<TipoServico>? TiposServico { get; set; }
}
