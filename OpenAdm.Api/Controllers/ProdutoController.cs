using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

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

    [ResponseCache(CacheProfileName = "Default300")]
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

    [HttpGet("all-list")]
    public async Task<IActionResult> ListAllProdutos()
    {
        try
        {
            var result = await _produtoService.GetAllProdutosAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [ResponseCache(CacheProfileName = "Default300")]
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

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    public async Task<IActionResult> CreateProduto([FromBody]CreateProdutoDto createProdutoDto)
    {
        try
        {
            var produtoViewModel = await _produtoService.CreateProdutoAsync(createProdutoDto);
            return Ok(produtoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("get-produto")]
    public async Task<IActionResult> GetProduto([FromQuery] Guid id)
    {
        try
        {
            var produtoViewModel = await _produtoService.GetProdutoViewModelByIdAsync(id);
            return Ok(produtoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpDelete("delete")]
    [IsFuncionario]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteProduto([FromQuery] Guid id)
    {
        try
        {
            await _produtoService.DeleteProdutoAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpPut("update")]
    [IsFuncionario]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UpdateProduto(UpdateProdutoDto updateProdutoDto)
    {
        try
        {
            var produtoViewlModel = await _produtoService.UpdateProdutoAsync(updateProdutoDto);
            return Ok(produtoViewlModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
