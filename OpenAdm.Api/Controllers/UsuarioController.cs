using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Tokens;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBaseApi
{
    private readonly IUsuarioService _usuarioService;
    private readonly ConfiguracaoDeToken _configGenerateToken;

    public UsuarioController(IUsuarioService usuarioService)
    {
        var key = VariaveisDeAmbiente.GetVariavel("JWT_KEY");
        var issue = VariaveisDeAmbiente.GetVariavel("JWT_ISSUE");
        var audience = VariaveisDeAmbiente.GetVariavel("JWT_AUDIENCE");
        var expirate = DateTime.Now.AddHours(int.Parse(VariaveisDeAmbiente.GetVariavel("JWT_EXPIRATION")));
        _configGenerateToken = new ConfiguracaoDeToken(key, issue, audience, expirate);
        _usuarioService = usuarioService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("get-conta")]
    public async Task<IActionResult> GetConta()
    {
        try
        {
            var usuarioViewModel = await _usuarioService.GetUsuarioByIdAsync();
            return Ok(usuarioViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUsuario(UpdateUsuarioDto updateUsuarioDto)
    {
        try
        {
            var result = await _usuarioService.UpdateUsuarioAsync(updateUsuarioDto, _configGenerateToken);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
