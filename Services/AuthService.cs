using System.Security.Cryptography;
using System.Text;
using GestaoHigienizePrime.Models;
using GestaoHigienizePrime.Repositories;

namespace GestaoHigienizePrime.Services;

public class AuthService : IAuthService
{
    private readonly IGoogleSheetsRepository _repository;

    public AuthService(IGoogleSheetsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Usuario?> AuthenticateAsync(string username, string password)
    {
        var usuarios = await _repository.GetUsuariosAsync();
        var usuario = usuarios.FirstOrDefault(u =>
            u.NomeUsuario?.Equals(username, StringComparison.OrdinalIgnoreCase) == true && u.Ativo);

        if (usuario == null) return null;

        var passwordHash = HashPassword(password);
        if (usuario.SenhaHash != passwordHash) return null;

        return usuario;
    }

    public async Task<bool> CreateUserAsync(Usuario usuario, string password)
    {
        if (await UserExistsAsync(usuario.NomeUsuario!)) return false;

        usuario.SenhaHash = HashPassword(password);
        usuario.Id = Guid.NewGuid().ToString();
        usuario.DataCriacao = DateTime.Now;
        usuario.Ativo = true;

        return await _repository.InsertUsuarioAsync(usuario);
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        var usuarios = await _repository.GetUsuariosAsync();
        return usuarios.Any(u => u.NomeUsuario?.Equals(username, StringComparison.OrdinalIgnoreCase) == true);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
