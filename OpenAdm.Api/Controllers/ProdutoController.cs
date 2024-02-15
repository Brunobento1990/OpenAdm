using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

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
}
