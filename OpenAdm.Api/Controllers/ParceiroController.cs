using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Parceiros;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("parceiro")]
[AcessoParceiro]
public class ParceiroController : ControllerBase
{
    private readonly IParceiroServico _parceiroServico;

    public ParceiroController(IParceiroServico parceiroServico)
    {
        _parceiroServico = parceiroServico;
    }

    [HttpPut("editar")]
    [ProducesResponseType<ParceiroViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Editar(ParceiroDto parceiroDto)
    {
        var response = await _parceiroServico.EditarAsync(parceiroDto);
        return Ok(response);
    }

    [HttpGet("obter")]
    [ProducesResponseType<ParceiroViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Obter()
    {
        var response = await _parceiroServico.ObterParceiroAutenticadoAsync();
        return Ok(response);
    }

    [HttpDelete("telefone")]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ExcluirTelefone([FromQuery] Guid telefoneId)
    {
        var resultado = await _parceiroServico.ExcluirTelefoneAsync(telefoneId);
        return Ok(new
        {
            resultado
        });
    }

    [HttpDelete("rede-social")]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ExcluirRedeSocial([FromQuery] Guid redeSocialId)
    {
        var resultado = await _parceiroServico.ExcluirRedeSocialAsync(redeSocialId);
        return Ok(new
        {
            resultado
        });
    }
}
