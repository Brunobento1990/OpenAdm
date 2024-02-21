using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("produtos")]
public class ProdutoController : ControllerBaseApi
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListProdutos([FromQuery] int page)
    {
        try
        {
            var result = await _produtoService.GetProdutosAsync(page);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("list-by-categorias")]
    public async Task<IActionResult> ListProdutosByCategorias([FromQuery] Guid categoriaId)
    {
        try
        {
            var result = await _produtoService.GetProdutosByCategoriaIdAsync(categoriaId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("paginacao")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    public async Task<IActionResult> ProdutoPaginacao([FromQuery] PaginacaoProdutoDto paginacaoProdutoDto)
    {
        try
        {
            var paginacao = await _produtoService.GetPaginacaoAsync(paginacaoProdutoDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
