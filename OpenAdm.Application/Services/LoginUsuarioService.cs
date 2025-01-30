using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class LoginUsuarioService(ILoginUsuarioRepository loginUsuarioRepository, ITokenService tokenService, IAcessoEcommerceService acessoEcommerceService)
    : ILoginUsuarioService
{
    private readonly ILoginUsuarioRepository _loginUsuarioRepository = loginUsuarioRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IAcessoEcommerceService _acessoEcommerceService = acessoEcommerceService;

    public async Task<ResponseLoginUsuarioViewModel> LoginAsync(RequestLogin requestLogin)
    {
        if (string.IsNullOrWhiteSpace(requestLogin.Email))
        {
            throw new ExceptionApi("CPF/CNPJ inválidos");
        }
        if (string.IsNullOrWhiteSpace(requestLogin.Senha))
        {
            throw new ExceptionApi("Senha inválida");
        }
        var usuario = await _loginUsuarioRepository.LoginAsync(requestLogin.Email.Replace(".", "").Replace("-", "").Replace("/", ""));

        if (usuario == null || !PasswordAdapter.VerifyPassword(requestLogin.Senha, usuario.Senha))
            throw new ExceptionApi("Usuário ou senha inválidos!");

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel);
        var refreshToken = _tokenService.GenerateRefreshToken(usuarioViewModel.Id);

        return new(usuarioViewModel, token, refreshToken);
    }

    public async Task<ResponseLoginUsuarioViewModel> LoginV2Async(RequestLoginUsuario requestLogin)
    {
        requestLogin.Validar();
        var usuario = await _loginUsuarioRepository.LoginAsync(requestLogin.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""));

        if (usuario == null || !PasswordAdapter.VerifyPassword(requestLogin.Senha, usuario.Senha))
            throw new ExceptionApi("Usuário ou senha inválidos!");

        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel);
        var refreshToken = _tokenService.GenerateRefreshToken(usuarioViewModel.Id);
        await _acessoEcommerceService.AtualizarAcessoAsync();
        return new(usuarioViewModel, token, refreshToken);
    }
}
