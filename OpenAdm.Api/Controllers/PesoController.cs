using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pesos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pesos")]
public class PesoController : ControllerBaseApi
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
        try
        {
            var pesosViewModel = await _pesoService.GetPesosViewModelAsync();
            return Ok(pesosViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoPesoDto paginacaoPesoDto)
    {
        try
        {
            var paginacao = await _pesoService.GetPaginacaoAsync(paginacaoPesoDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("get-peso")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    public async Task<IActionResult> GetPeso([FromQuery] Guid id)
    {
        try
        {
            var pesoViewModel = await _pesoService.GetPesoViewModelAsync(id);
            return Ok(pesoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    public async Task<IActionResult> CreatePeso(CreatePesoDto createPesoDto)
    {
        try
        {
            var pesoViewModel = await _pesoService.CreatePesoAsync(createPesoDto);
            return Ok(pesoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeletePeso([FromQuery] Guid id)
    {
        try
        {
            await _pesoService.DeletePesoAsync(id);
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
    public async Task<IActionResult> UpdatePeso(UpdatePesoDto updatePesoDto)
    {
        try
        {
            var pesoViewlModel = await _pesoService.UpdatePesoAsync(updatePesoDto);
            return Ok(pesoViewlModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
