using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pesos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pesos")]
public class PesoController : ControllerBase
{
    private readonly IPesoService _pesoService;

    public PesoController(IPesoService pesoService)
    {
        _pesoService = pesoService;
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var pesosViewModel = await _pesoService.GetPesosViewModelAsync();
        return Ok(pesosViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoPesoDto paginacaoPesoDto)
    {
        var paginacao = await _pesoService.GetPaginacaoAsync(paginacaoPesoDto);
        return Ok(paginacao);
    }

    [HttpGet("get-peso")]
    [Autentica]
    [IsFuncionario]
    public async Task<IActionResult> GetPeso([FromQuery] Guid id)
    {
        var pesoViewModel = await _pesoService.GetPesoViewModelAsync(id);
        return Ok(pesoViewModel);
    }

    [HttpPost("create")]
    [Autentica]
    [IsFuncionario]
    public async Task<IActionResult> CreatePeso(CreatePesoDto createPesoDto)
    {
        var pesoViewModel = await _pesoService.CreatePesoAsync(createPesoDto);
        return Ok(pesoViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeletePeso([FromQuery] Guid id)
    {
        await _pesoService.DeletePesoAsync(id);
        return Ok();
    }

    [Autentica]
    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> UpdatePeso(UpdatePesoDto updatePesoDto)
    {
        var pesoViewlModel = await _pesoService.UpdatePesoAsync(updatePesoDto);
        return Ok(pesoViewlModel);
    }
}
