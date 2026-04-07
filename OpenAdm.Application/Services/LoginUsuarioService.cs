using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class LoginUsuarioService(
    ILoginUsuarioRepository loginUsuarioRepository,
    ITokenService tokenService,
    IAcessoEcommerceService acessoEcommerceService)
    : ILoginUsuarioService
{
    private readonly ILoginUsuarioRepository _loginUsuarioRepository = loginUsuarioRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IAcessoEcommerceService _acessoEcommerceService = acessoEcommerceService;

    public async Task<ResponseLoginUsuarioViewModel> LoginGoogleAsync(RequestLoginGoogle requestLoginGoogle)
    {
        var resultadoToken = await _tokenService.ValidarTokenGoogleAsync(requestLoginGoogle.Jwt);

        var usuario = await _loginUsuarioRepository.LoginComGoogleAsync(resultadoToken.Email)
                      ?? throw new ExceptionApi("Não foi possível localizar seu cadastro");

        if (!usuario.Ativo)
        {
            throw new ExceptionApi("Usuário inativo. Entre em contato com o administrador do sistema.");
        }

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuario.Id, false);
        var refreshToken = _tokenService.GenerateRefreshToken(usuario.Id, false);

        return new(usuarioViewModel, token, refreshToken);
    }

    public async Task<ResponseLoginUsuarioViewModel> LoginV2Async(RequestLoginUsuario requestLogin)
    {
        requestLogin.Validar();
        var usuario =
            await _loginUsuarioRepository.LoginAsync(requestLogin.CpfCnpj.Replace(".", "").Replace("-", "")
                .Replace("/", ""));


        if (usuario == null || !PasswordAdapter.VerifyPassword(requestLogin.Senha, usuario.Senha))
            throw new ExceptionApi("Usuário ou senha inválidos!");

        if (!usuario.Ativo)
            throw new ExceptionApi("Usuário inativo. Entre em contato com o administrador do sistema.");

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuario.Id, false);
        var refreshToken = _tokenService.GenerateRefreshToken(usuario.Id, false);
        await _acessoEcommerceService.AtualizarAcessoAsync();
        return new(usuarioViewModel, token, refreshToken);
    }
}