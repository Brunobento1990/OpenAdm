using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Tamanhos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("tamanhos")]
public class TamanhoController : ControllerBaseApi
{
    private readonly ITamanhoService _tamanhoService;

    public TamanhoController(ITamanhoService tamanhoService)
    {
        _tamanhoService = tamanhoService;
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        try
        {
            var tamanhosViewModel = await _tamanhoService.GetTamanhoViewModelsAsync();
            return Ok(tamanhosViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoTamanhoDto paginacaoTamanhoDto)
    {
        try
        {
            var paginacao = await _tamanhoService.GetPaginacaoAsync(paginacaoTamanhoDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("get-tamanho")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        try
        {
            var tamanhoViewModel = await _tamanhoService.GetTamanhoViewModelAsync(id);
            return Ok(tamanhoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteTamanho([FromQuery] Guid id)
    {
        try
        {
            await _tamanhoService.DeleteTamanhoAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateTamanho(UpdateTamanhoDto updateTamanhoDto)
    {
        try
        {
            var tamanhoViewlModel = await _tamanhoService.UpdateTamanhoAsync(updateTamanhoDto);
            return Ok(tamanhoViewlModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpPost("create")]
    public async Task<IActionResult> CreateTamanho(CreateTamanhoDto createTamanhoDto)
    {
        try
        {
            var tamanhoViewModel = await _tamanhoService.CreateTamanhoAsync(createTamanhoDto);
            return Ok(tamanhoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
