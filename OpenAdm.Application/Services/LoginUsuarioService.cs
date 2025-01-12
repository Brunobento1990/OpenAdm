using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class LoginUsuarioService(ILoginUsuarioRepository loginUsuarioRepository, ITokenService tokenService)
    : ILoginUsuarioService
{
    private readonly ILoginUsuarioRepository _loginUsuarioRepository = loginUsuarioRepository;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<ResponseLoginUsuarioViewModel> LoginAsync(RequestLoginUsuario requestLogin)
    {
        requestLogin.Validar();
        var usuario = await _loginUsuarioRepository.LoginAsync(requestLogin.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""));

        if (usuario == null || !PasswordAdapter.VerifyPassword(requestLogin.Senha, usuario.Senha))
            throw new ExceptionApi("Usuário ou senha inválidos!");

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel);
        var refreshToken = _tokenService.GenerateRefreshToken(usuarioViewModel.Id);

        return new(usuarioViewModel, token, refreshToken);
    }
}
