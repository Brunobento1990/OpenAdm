using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("usuarios")]
public class EsqueceuSenhaController : ControllerBaseApi
{
    private readonly IEsqueceuSenhaService _esqueceuSenhaService;

    public EsqueceuSenhaController(IEsqueceuSenhaService esqueceuSenhaService)
    {
        _esqueceuSenhaService = esqueceuSenhaService;
    }

    [HttpPut("esqueceu-senha")]
    public async Task<IActionResult> ResetarSenha(EsqueceuSenhaDto esqueceuSenhaDto)
    {
        try
        {
            await _esqueceuSenhaService.RecuperarSenhaAsync(esqueceuSenhaDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
