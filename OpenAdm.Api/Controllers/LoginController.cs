using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Tokens;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBaseApi
{
    private readonly ILoginFuncionarioService _loginFuncionarioService;
    private readonly ILoginUsuarioService _loginUsuarioService;
    private readonly ConfiguracaoDeToken _configGenerateToken;

    public LoginController(
        ILoginFuncionarioService loginFuncionarioService, 
        ILoginUsuarioService loginUsuarioService)
    {
        var key = VariaveisDeAmbiente.GetVariavel("JWT_KEY");
        var issue = VariaveisDeAmbiente.GetVariavel("JWT_ISSUE");
        var audience = VariaveisDeAmbiente.GetVariavel("JWT_AUDIENCE");
        var expirate = DateTime.Now.AddHours(int.Parse(VariaveisDeAmbiente.GetVariavel("JWT_EXPIRATION")));
        _configGenerateToken = new ConfiguracaoDeToken(key, issue, audience, expirate);
        _loginFuncionarioService = loginFuncionarioService;
        _loginUsuarioService = loginUsuarioService;
    }

    [HttpPost("funcionario")]
    public async Task<IActionResult> LoginFuncionario(RequestLogin requestLogin)
    {
        try
        {
            var responselogin = await _loginFuncionarioService.LoginFuncionarioAsync(requestLogin, _configGenerateToken);
            return Ok(responselogin);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpPost("usuario")]
    public async Task<IActionResult> LoginUsuario(RequestLogin requestLogin)
    {
        try
        {
            var responselogin = await _loginUsuarioService.LoginAsync(requestLogin, _configGenerateToken);
            return Ok(responselogin);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
