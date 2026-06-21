using GestaoHigienizePrime.Models;

namespace GestaoHigienizePrime.Services;

public interface IAuthService
{
    Task<Usuario?> AuthenticateAsync(string username, string password);
    Task<bool> CreateUserAsync(Usuario usuario, string password);
    Task<bool> UserExistsAsync(string username);
}
