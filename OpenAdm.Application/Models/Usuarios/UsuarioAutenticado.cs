using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.Usuarios;

public sealed class UsuarioAutenticado : IUsuarioAutenticado
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IFuncionarioRepository _funcionarioRepository;
    private readonly IRepresentanteRepository _representanteRepository;

    public UsuarioAutenticado(
        IUsuarioRepository usuarioRepository,
        IFuncionarioRepository funcionarioRepository,
        IRepresentanteRepository representanteRepository)
    {
        _usuarioRepository = usuarioRepository;
        _funcionarioRepository = funcionarioRepository;
        _representanteRepository = representanteRepository;
    }

    public Guid Id { get; set; }
    public Guid SessaoId { get; set; }
    public Guid ParceiroId { get; set; }
    public TipoUsuario TipoUsuario { get; set; }

    public async Task<Usuario> GetUsuarioAutenticadoAsync()
    {
        return await _usuarioRepository
                   .GetUsuarioByIdAsync(Id)
               ?? throw new UnauthorizedAccessException("Usuário não autenticado!");
    }

    public async Task<Usuario?> GetUsuarioMiddlewareAsync()
    {
        return await _usuarioRepository
                   .GetUsuarioMiddlewareAsync(Id);
    }

    public async Task<Funcionario?> GetFuncionarioMiddlewareAsync()
    {
        return await _funcionarioRepository.ObterPorIdMiddlewareAsync(Id, ParceiroId);
    }

    public Task<Representante?> GetRepresentanteMiddlewareAsync() =>
        _representanteRepository.ObterPorIdAsync(Id);

    public async Task<Usuario?> GetUsuarioAutenticadoOrNullAsync()
    {
        if (Id == Guid.Empty)
        {
            return null;
        }

        return await _usuarioRepository
            .GetUsuarioByIdAsync(Id);
    }
}
