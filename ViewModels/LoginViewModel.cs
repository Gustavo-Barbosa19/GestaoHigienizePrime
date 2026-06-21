using System.ComponentModel.DataAnnotations;

namespace GestaoHigienizePrime.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Usuário é obrigatório")]
    public string? Usuario { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }

    public bool LembrarMe { get; set; }
}
