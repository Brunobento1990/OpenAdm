using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Dtos.Funcionarios;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("funcionarios")]
[AcessoParceiro]
public class FuncionarioEsqueceuSenhaController(IFuncionarioEsqueceuSenhaService service) : ControllerBase
{
    [HttpPost("esqueceu-senha")]
    [ProducesResponseType<ResultadoPadraoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Solicitar(EsqueceuSenhaDto esqueceuSenhaDto)
    {
        var resultado = await service.SolicitarAsync(esqueceuSenhaDto);
        return resultado.ToActionResult();
    }

    [HttpPut("recuperar-senha")]
    [ProducesResponseType<ResultadoPadraoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> RecuperarSenha(RecuperarSenhaFuncionarioDto dto)
    {
        var resultado = await service.RecuperarSenhaAsync(dto);
        return resultado.ToActionResult();
    }
}
