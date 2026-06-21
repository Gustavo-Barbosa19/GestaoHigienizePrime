using System.ComponentModel.DataAnnotations;

namespace GestaoHigienizePrime.Models;

public class Cliente
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 200 caracteres")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Telefone é obrigatório")]
    public string? Telefone { get; set; }

    public string? WhatsApp { get; set; }

    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string? Email { get; set; }

    public string? CEP { get; set; }
    public string? Endereco { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public bool Ativo { get; set; } = true;
}
