using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using static BCrypt.Net.BCrypt;

namespace OpenAdm.Application.Services;

public class LoginUsuarioService(ILoginUsuarioRepository loginUsuarioRepository, ITokenService tokenService)
    : ILoginUsuarioService
{
    private readonly ILoginUsuarioRepository _loginUsuarioRepository = loginUsuarioRepository;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<ResponseLoginUsuarioViewModel> LoginAsync(RequestLogin requestLogin, ConfiguracaoDeToken configuracaoDeToken)
    {
        var usuario = await _loginUsuarioRepository.LoginAsync(requestLogin.Email);

        if (usuario == null || !Verify(requestLogin.Senha, usuario.Senha))
            throw new ExceptionApi(CodigoErrors.ErrorEmailOuSenhaInvalido);

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel, configuracaoDeToken);

        return new(usuarioViewModel, token);
    }
}
