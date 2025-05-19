using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.LojasParceiras;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.LojasParceira;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("lojas-parceiras")]
[AutenticaParceiro]
public class LojasParceirasController : ControllerBase
{
    private readonly ILojasParceirasService _lojasParceirasService;

    public LojasParceirasController(ILojasParceirasService lojasParceirasService)
    {
        _lojasParceirasService = lojasParceirasService;
    }

    [HttpPost("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<LojasParceirasViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Paginacao(PaginacaoLojasParceirasDto paginacaoLojasParceirasDto)
    {
        var paginacao = await _lojasParceirasService.GetPaginacaoAsync(paginacaoLojasParceirasDto);
        return Ok(paginacao);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("create")]
    [ProducesResponseType<LojasParceirasViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Create(CreateLojaParceiraDto createLojaParceiraDto)
    {
        var lojaParceiraViewModel = await _lojasParceirasService.CreateLojaParceiraAsync(createLojaParceiraDto);
        return Ok(lojaParceiraViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPut("update")]
    [ProducesResponseType<LojasParceirasViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Update(UpdateLojaParceiraDto updateLojaParceiraDto)
    {
        var lojaParceiraViewModel = await _lojasParceirasService.UpdateLojaParceiraAsync(updateLojaParceiraDto);
        return Ok(lojaParceiraViewModel);
    }

    [HttpGet("get-loja")]
    [ProducesResponseType<LojasParceirasViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GetLoja([FromQuery] Guid id)
    {
        var lojaParceiraViewModel = await _lojasParceirasService.GetLojasParceirasViewModelAsync(id);
        return Ok(lojaParceiraViewModel);
    }
    [Autentica]
    [IsFuncionario]
    [HttpDelete("delete")]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> DeleteLoja([FromQuery] Guid id)
    {
        await _lojasParceirasService.DeleteLojaParceiraAsync(id);
        return Ok();
    }

    [HttpGet("list")]
    [ResponseCache(CacheProfileName = "Default300")]
    [ProducesResponseType<IList<string>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> List()
    {
        var response = await _lojasParceirasService.ListLojasParceirasViewModelAsync();
        return Ok(response);
    }

    [HttpGet("todas")]
    [ResponseCache(CacheProfileName = "Default300")]
    [ProducesResponseType<IList<LojasParceirasViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Todas()
    {
        var response = await _lojasParceirasService.TodasLojasAsync();
        return Ok(response);
    }
}
