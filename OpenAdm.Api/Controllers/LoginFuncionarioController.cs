using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("login")]
public class LoginFuncionarioController : ControllerBaseApi
{
    private readonly ILoginFuncionarioService _loginFuncionarioService;
    private readonly ConfiguracaoDeToken _configGenerateToken;

    public LoginFuncionarioController(ILoginFuncionarioService loginFuncionarioService)
    {
        var key = VariaveisDeAmbiente.GetVariavel("JWT_KEY");
        var issue = VariaveisDeAmbiente.GetVariavel("JWT_ISSUE");
        var audience = VariaveisDeAmbiente.GetVariavel("JWT_AUDIENCE");
        var expirate = DateTime.Now.AddHours(int.Parse(VariaveisDeAmbiente.GetVariavel("JWT_EXPIRATION")));
        _configGenerateToken = new ConfiguracaoDeToken(key, issue, audience, expirate);
        _loginFuncionarioService = loginFuncionarioService;
    }

    [HttpPost("funcionario")]
    public async Task<IActionResult> Login(RequestLogin requestLogin)
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
}
