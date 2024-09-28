using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("tabelas-de-precos")]
[IsFuncionario]
[Autentica]
[AutenticaParceiro]
public class TabelaDePrecoController : ControllerBase
{
    private readonly ITabelaDePrecoService _tabelaDePrecoService;

    public TabelaDePrecoController(ITabelaDePrecoService tabelaDePrecoService)
    {
        _tabelaDePrecoService = tabelaDePrecoService;
    }

    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoTabelaDePrecoDto paginacaoTabelaDePrecoDto)
    {
        var paginacao = await _tabelaDePrecoService.GetPaginacaoTabelaViewModelAsync(paginacaoTabelaDePrecoDto);
        return Ok(paginacao);
    }

    [HttpGet("get-tabela")]
    public async Task<IActionResult> TabelaViewModel([FromQuery] Guid id)
    {
        var tabelaDePrecoViewModel = await _tabelaDePrecoService.GetPrecoTabelaViewModelAsync(id);
        return Ok(tabelaDePrecoViewModel);
    }

    [HttpGet("get-tabela-ativa")]
    public async Task<IActionResult> TabelaViewModelAtiva()
    {
        var tabelaDePrecoViewModel = await _tabelaDePrecoService.GetTabelaDePrecoViewModelAtivaAsync();
        return Ok(tabelaDePrecoViewModel);
    }

    [HttpGet("get-tabela-by-produtoId")]
    public async Task<IActionResult> TabelaViewModelByProdutoId([FromQuery] Guid produtoId)
    {
        var tabelaDePrecoViewModel = await _tabelaDePrecoService.GetTabelaViewModelByProdutoIdAsync(produtoId);
        return Ok(tabelaDePrecoViewModel);
    }

    [HttpGet("list")]
    public async Task<IActionResult> TabelasViewModel()
    {
        var tabelaDePrecoViewModel = await _tabelaDePrecoService.GetAllTabelaDePrecoViewModelAsync();
        return Ok(tabelaDePrecoViewModel);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTabelaDePreco(CreateTabelaDePrecoDto createTabelaDePrecoDto)
    {
        var tabelaDePrecoViewModel = await _tabelaDePrecoService.CreateTabelaDePrecoAsync(createTabelaDePrecoDto);
        return Ok(tabelaDePrecoViewModel);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTabela(UpdateTabelaDePrecoDto updateTabelaDePrecoDto)
    {
        var tabelaDePrecoViewModel = await _tabelaDePrecoService.UpdateTabelaDePrecoAsync(updateTabelaDePrecoDto);
        return Ok(tabelaDePrecoViewModel);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteTabelaDePreco([FromQuery] Guid id)
    {
        await _tabelaDePrecoService.DeleteTabelaDePrecoAsync(id);
        return Ok();
    }
}
