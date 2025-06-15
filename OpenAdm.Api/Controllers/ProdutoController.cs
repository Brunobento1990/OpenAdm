using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("produtos")]
[AcessoParceiro]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet("list")]
    [TryAutentica]
    [ProducesResponseType<PaginacaoViewModel<ProdutoViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ListProdutos([FromQuery] PaginacaoProdutoEcommerceDto paginacaoProdutoEcommerceDto)
    {
        var result = await _produtoService.GetProdutosAsync(paginacaoProdutoEcommerceDto);
        return Ok(result);
    }

    [HttpGet("all-list")]
    [ProducesResponseType<IList<ProdutoViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ListAllProdutos()
    {
        var result = await _produtoService.GetAllProdutosAsync();
        return Ok(result);
    }

    [HttpGet("list-by-categorias")]
    [TryAutentica]
    [ProducesResponseType<IList<ProdutoViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ListProdutosByCategorias([FromQuery] Guid categoriaId)
    {
        var result = await _produtoService.GetProdutosByCategoriaIdAsync(categoriaId);
        return Ok(result);
    }

    [HttpPost("paginacao")]
    [Autentica]
    [IsFuncionario]
    [ProducesResponseType<PaginacaoViewModel<ProdutoViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ProdutoPaginacao(PaginacaoProdutoDto paginacaoProdutoDto)
    {
        var paginacao = await _produtoService.GetPaginacaoAsync(paginacaoProdutoDto);
        return Ok(paginacao);
    }

    [HttpPost("paginacao-drop-down")]
    [Autentica]
    [IsFuncionario]
    [ProducesResponseType<IEnumerable<ProdutoViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ProdutoPaginacaoDropDown(PaginacaoProdutoDropDown paginacaoProdutoDropDown)
    {
        var paginacao = await _produtoService.GetDropDownPaginacaoAsync(paginacaoProdutoDropDown);
        return Ok(paginacao);
    }

    [HttpPost("create")]
    [Autentica]
    [IsFuncionario]
    [ProducesResponseType<ProdutoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoDto createProdutoDto)
    {
        var produtoViewModel = await _produtoService.CreateProdutoAsync(createProdutoDto);
        return Ok(produtoViewModel);
    }

    [HttpGet("get-produto")]
    [ProducesResponseType<ProdutoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GetProduto([FromQuery] Guid id)
    {
        var produtoViewModel = await _produtoService.GetProdutoViewModelByIdAsync(id);
        return Ok(produtoViewModel);
    }

    [HttpDelete("delete")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> DeleteProduto([FromQuery] Guid id)
    {
        await _produtoService.DeleteProdutoAsync(id);
        return Ok();
    }

    [HttpPut("update")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType<PesoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> UpdateProduto(UpdateProdutoDto updateProdutoDto)
    {
        var produtoViewlModel = await _produtoService.UpdateProdutoAsync(updateProdutoDto);
        return Ok(produtoViewlModel);
    }

    [HttpPut("inativar-ativar")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> InativarAtivar([FromQuery] Guid id)
    {
        await _produtoService.InativarAtivarEcommerceAsync(id);
        return Ok(new
        {
            result = true
        });
    }
}
