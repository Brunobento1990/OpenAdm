using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Tamanhos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("tamanhos")]
[Authorize(AuthenticationSchemes = "Bearer")]
[IsFuncionario]
public class TamanhoController : ControllerBaseApi
{
    private readonly ITamanhoService _tamanhoService;

    public TamanhoController(ITamanhoService tamanhoService)
    {
        _tamanhoService = tamanhoService;
    }

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
