using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("usuarios")]
public class EsqueceuSenhaController : ControllerBase
{
    private readonly IEsqueceuSenhaService _esqueceuSenhaService;

    public EsqueceuSenhaController(IEsqueceuSenhaService esqueceuSenhaService)
    {
        _esqueceuSenhaService = esqueceuSenhaService;
    }

    [HttpPut("esqueceu-senha")]
    public async Task<IActionResult> ResetarSenha(EsqueceuSenhaDto esqueceuSenhaDto)
    {
        await _esqueceuSenhaService.RecuperarSenhaAsync(esqueceuSenhaDto);
        return Ok();
    }
}
