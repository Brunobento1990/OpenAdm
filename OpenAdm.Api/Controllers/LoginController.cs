using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("login")]
[AcessoParceiro]
[UsuarioSessao]
public class LoginController : ControllerBase
{
    private readonly ILoginFuncionarioService _loginFuncionarioService;
    private readonly ILoginUsuarioService _loginUsuarioService;
    private readonly ISessaoUsuarioService _sessaoUsuarioService;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public LoginController(
        ILoginFuncionarioService loginFuncionarioService,
        ILoginUsuarioService loginUsuarioService,
        ISessaoUsuarioService sessaoUsuarioService,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _loginFuncionarioService = loginFuncionarioService;
        _loginUsuarioService = loginUsuarioService;
        _sessaoUsuarioService = sessaoUsuarioService;
        _usuarioAutenticado = usuarioAutenticado;
    }

    [HttpPost("funcionario")]
    [ProducesResponseType<ResponseLoginFuncionarioViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> LoginFuncionario(RequestLogin requestLogin)
    {
        var responselogin = await _loginFuncionarioService.LoginFuncionarioAsync(requestLogin);
        return Ok(responselogin);
    }

    [HttpPost("usuario/google")]
    [ProducesResponseType<ResponseLoginUsuarioViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> LoginUsuarioGoogle(RequestLoginGoogle requestLoginGoogle)
    {
        var responselogin = await _loginUsuarioService.LoginGoogleAsync(requestLoginGoogle);
        return Ok(responselogin);
    }

    [HttpPost("usuario-v2")]
    [ProducesResponseType<ResponseLoginUsuarioViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> LoginUsuarioV2(RequestLoginUsuario requestLogin)
    {
        var responselogin = await _loginUsuarioService.LoginV2Async(requestLogin);
        return Ok(responselogin);
    }

    [HttpPost("logout")]
    [Autentica]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(401)]
    public async Task<IActionResult> Logout()
    {
        await _sessaoUsuarioService.DerrubarSessaoAsync(_usuarioAutenticado.SessaoId);
        return Ok(new { result = true });
    }
}
