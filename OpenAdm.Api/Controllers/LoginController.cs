using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("login")]
[AutenticaParceiro]
public class LoginController : ControllerBase
{
    private readonly ILoginFuncionarioService _loginFuncionarioService;
    private readonly ILoginUsuarioService _loginUsuarioService;

    public LoginController(
        ILoginFuncionarioService loginFuncionarioService,
        ILoginUsuarioService loginUsuarioService)
    {
        _loginFuncionarioService = loginFuncionarioService;
        _loginUsuarioService = loginUsuarioService;
    }

    [HttpPost("funcionario")]
    public async Task<IActionResult> LoginFuncionario(RequestLogin requestLogin)
    {
        var responselogin = await _loginFuncionarioService.LoginFuncionarioAsync(requestLogin);
        return Ok(responselogin);
    }

    [HttpPost("usuario")]
    [ProducesResponseType<ResponseLoginUsuarioViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> LoginUsuario(RequestLogin requestLogin)
    {
        var responselogin = await _loginUsuarioService.LoginAsync(requestLogin);
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
}
