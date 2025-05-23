using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pesos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pesos")]
[AcessoParceiro]
public class PesoController : ControllerBase
{
    private readonly IPesoService _pesoService;

    public PesoController(IPesoService pesoService)
    {
        _pesoService = pesoService;
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("list")]
    [ProducesResponseType<IList<PesoViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> List()
    {
        var pesosViewModel = await _pesoService.GetPesosViewModelAsync();
        return Ok(pesosViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<PesoViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Paginacao(PaginacaoPesoDto paginacaoPesoDto)
    {
        var paginacao = await _pesoService.GetPaginacaoAsync(paginacaoPesoDto);
        return Ok(paginacao);
    }

    [HttpGet("get-peso")]
    [Autentica]
    [IsFuncionario]
    [ProducesResponseType<PesoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GetPeso([FromQuery] Guid id)
    {
        var pesoViewModel = await _pesoService.GetPesoViewModelAsync(id);
        return Ok(pesoViewModel);
    }

    [HttpPost("create")]
    [Autentica]
    [IsFuncionario]
    [ProducesResponseType<PesoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CreatePeso(CreatePesoDto createPesoDto)
    {
        var pesoViewModel = await _pesoService.CreatePesoAsync(createPesoDto);
        return Ok(pesoViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpDelete("delete")]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> DeletePeso([FromQuery] Guid id)
    {
        await _pesoService.DeletePesoAsync(id);
        return Ok();
    }

    [Autentica]
    [IsFuncionario]
    [HttpPut("update")]
    [ProducesResponseType<PesoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> UpdatePeso(UpdatePesoDto updatePesoDto)
    {
        var pesoViewlModel = await _pesoService.UpdatePesoAsync(updatePesoDto);
        return Ok(pesoViewlModel);
    }
}
