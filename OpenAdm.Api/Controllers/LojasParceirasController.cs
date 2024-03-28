using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.LojaParceira;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("lojas-parceiras")]
public class LojasParceirasController : ControllerBaseApi
{
    private readonly ILojasParceirasService _lojasParceirasService;

    public LojasParceirasController(ILojasParceirasService lojasParceirasService)
    {
        _lojasParceirasService = lojasParceirasService;
    }

    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoLojasParceirasDto paginacaoLojasParceirasDto)
    {
        try
        {
            var paginacao = await _lojasParceirasService.GetPaginacaoAsync(paginacaoLojasParceirasDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateLojaParceiraDto createLojaParceiraDto)
    {
        try
        {
            var lojaParceiraViewModel = await _lojasParceirasService.CreateLojaParceiraAsync(createLojaParceiraDto);
            return Ok(lojaParceiraViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateLojaParceiraDto updateLojaParceiraDto)
    {
        try
        {
            var lojaParceiraViewModel = await _lojasParceirasService.UpdateLojaParceiraAsync(updateLojaParceiraDto);
            return Ok(lojaParceiraViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("get-loja")]
    public async Task<IActionResult> GetLoja([FromQuery] Guid id)
    {
        try
        {
            var lojaParceiraViewModel = await _lojasParceirasService.GetLojasParceirasViewModelAsync(id);
            return Ok(lojaParceiraViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteLoja([FromQuery] Guid id)
    {
        try
        {
            await _lojasParceirasService.DeleteLojaParceiraAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("list")]
    [ResponseCache(CacheProfileName = "Default300")]
    public async Task<IActionResult> List()
    {
        try
        {
            var response = await _lojasParceirasService.ListLojasParceirasViewModelAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
