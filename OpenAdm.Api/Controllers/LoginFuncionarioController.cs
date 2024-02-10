using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("login")]
public class LoginFuncionarioController : ControllerBaseApi
{
    private readonly ILoginFuncionarioService _loginFuncionarioService;

    public LoginFuncionarioController(ILoginFuncionarioService loginFuncionarioService)
    {
        _loginFuncionarioService = loginFuncionarioService;
    }

    [HttpPost("funcionario")]
    public async Task<IActionResult> Login(RequestLogin requestLogin)
    {
        try
        {
            var responselogin = await _loginFuncionarioService.LoginFuncionarioAsync(requestLogin);
            return Ok(responselogin);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
