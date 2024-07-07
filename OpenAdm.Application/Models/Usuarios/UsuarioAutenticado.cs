using Domain.Pkg.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Models.Usuarios;

public sealed class UsuarioAutenticado : IUsuarioAutenticado
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioAutenticado(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public Guid Id { get; set; }
    public bool IsFuncionario { get; set; }

    public async Task<Usuario> GetUsuarioAutenticadoAsync()
    {
        return await _usuarioRepository
            .GetUsuarioByIdAsync(Id)
            ?? throw new UnauthorizedAccessException("Usuário não autenticado!");
    }
}
