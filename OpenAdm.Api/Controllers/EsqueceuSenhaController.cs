using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("usuarios")]
[AutenticaParceiro]
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
        return Ok(new
        {
            result = true
        });
    }
}
