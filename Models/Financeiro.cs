using System.ComponentModel.DataAnnotations;
using GestaoHigienizePrime.Models.Enums;

namespace GestaoHigienizePrime.Models;

public class Financeiro
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Tipo é obrigatório")]
    public TipoTransacao Tipo { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatória")]
    public string? Categoria { get; set; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Data é obrigatória")]
    public DateTime Data { get; set; }

    public string? ServicoId { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
}
