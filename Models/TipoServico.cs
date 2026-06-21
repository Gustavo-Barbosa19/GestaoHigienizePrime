using System.ComponentModel.DataAnnotations;

namespace GestaoHigienizePrime.Models;

public class TipoServico
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    public string? Nome { get; set; }

    public string? Descricao { get; set; }
    public bool Ativo { get; set; } = true;
}
