using System.ComponentModel.DataAnnotations;
using GestaoHigienizePrime.Models.Enums;

namespace GestaoHigienizePrime.ViewModels;

public class FinanceiroViewModel
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
    public DateTime Data { get; set; } = DateTime.Now;

    public string? ServicoId { get; set; }

    public static List<string> CategoriasEntrada => new() { "Serviços Realizados", "Outros" };
    public static List<string> CategoriasSaida => new() { "Produtos", "Combustível", "Marketing", "Equipamentos", "Outros Gastos" };
}
