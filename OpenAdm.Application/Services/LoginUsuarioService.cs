using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;
using static BCrypt.Net.BCrypt;

namespace OpenAdm.Application.Services;

public class LoginUsuarioService(ILoginUsuarioRepository loginUsuarioRepository, ITokenService tokenService)
    : ILoginUsuarioService
{
    private readonly ILoginUsuarioRepository _loginUsuarioRepository = loginUsuarioRepository;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<ResponseLoginUsuarioViewModel> LoginAsync(RequestLogin requestLogin)
    {
        var usuario = await _loginUsuarioRepository.LoginAsync(requestLogin.Email);

        if (usuario == null || !Verify(requestLogin.Senha, usuario.Senha))
            throw new ExceptionApi(CodigoErrors.ErrorEmailOuSenhaInvalido);

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel);

        return new(usuarioViewModel, token);
    }
}
