using System.ComponentModel.DataAnnotations;

namespace GestaoHigienizePrime.Models;

public class Usuario
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Usuário é obrigatório")]
    public string? NomeUsuario { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string? SenhaHash { get; set; }

    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string? Email { get; set; }

    public string? NomeCompleto { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public bool Ativo { get; set; } = true;
}
