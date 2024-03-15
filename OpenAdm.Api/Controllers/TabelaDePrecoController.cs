using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[IsFuncionario]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("tabelas-de-precos")]
public class TabelaDePrecoController : ControllerBaseApi
{
    private readonly ITabelaDePrecoService _tabelaDePrecoService;

    public TabelaDePrecoController(ITabelaDePrecoService tabelaDePrecoService)
    {
        _tabelaDePrecoService = tabelaDePrecoService;
    }

    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoTabelaDePrecoDto paginacaoTabelaDePrecoDto)
    {
        try
        {
            var paginacao = await _tabelaDePrecoService.GetPaginacaoTabelaViewModelAsync(paginacaoTabelaDePrecoDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("get-tabela")]
    public async Task<IActionResult> TabelaViewModel([FromQuery] Guid id)
    {
        try
        {
            var tabelaDePrecoViewModel = await _tabelaDePrecoService.GetPrecoTabelaViewModelAsync(id);
            return Ok(tabelaDePrecoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("get-tabela-ativa")]
    public async Task<IActionResult> TabelaViewModelAtiva()
    {
        try
        {
            var tabelaDePrecoViewModel = await _tabelaDePrecoService.GetTabelaDePrecoViewModelAtivaAsync();
            return Ok(tabelaDePrecoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("get-tabela-by-produtoId")]
    public async Task<IActionResult> TabelaViewModelByProdutoId([FromQuery] Guid produtoId)
    {
        try
        {
            var tabelaDePrecoViewModel = await _tabelaDePrecoService.GetTabelaViewModelByProdutoIdAsync(produtoId);
            return Ok(tabelaDePrecoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> TabelasViewModel()
    {
        try
        {
            var tabelaDePrecoViewModel = await _tabelaDePrecoService.GetAllTabelaDePrecoViewModelAsync();
            return Ok(tabelaDePrecoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTabelaDePreco(CreateTabelaDePrecoDto createTabelaDePrecoDto)
    {
        try
        {
            var tabelaDePrecoViewModel = await _tabelaDePrecoService.CreateTabelaDePrecoAsync(createTabelaDePrecoDto);
            return Ok(tabelaDePrecoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTabela(UpdateTabelaDePrecoDto updateTabelaDePrecoDto)
    {
        try
        {
            var tabelaDePrecoViewModel = await _tabelaDePrecoService.UpdateTabelaDePrecoAsync(updateTabelaDePrecoDto);
            return Ok(tabelaDePrecoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteTabelaDePreco([FromQuery] Guid id)
    {
        try
        {
            await _tabelaDePrecoService.DeleteTabelaDePrecoAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
