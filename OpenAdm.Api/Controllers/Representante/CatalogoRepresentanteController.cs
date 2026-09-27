using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Application.Models.Tamanhos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers.Representante;

[ApiController]
[Route("representante")]
[AcessoParceiro]
[Autentica]
[IsRepresentante]
public sealed class CatalogoRepresentanteController(
    IProdutoService produtoService,
    IPesoService pesoService,
    ITamanhoService tamanhoService) : ControllerBase
{
    [HttpPost("produtos/paginacao")]
    [ProducesResponseType<PaginacaoViewModel<ProdutoViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PaginarProdutosAsync(PaginacaoProdutoDto filtro) =>
        Ok(await produtoService.GetPaginacaoAsync(filtro));

    [HttpPost("pesos/paginacao")]
    [ProducesResponseType<PaginacaoViewModel<PesoViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PaginarPesosAsync(PaginacaoPesoDto filtro) =>
        Ok(await pesoService.GetPaginacaoAsync(filtro));

    [HttpPost("tamanhos/paginacao")]
    [ProducesResponseType<PaginacaoViewModel<TamanhoViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PaginarTamanhosAsync(PaginacaoTamanhoDto filtro) =>
        Ok(await tamanhoService.GetPaginacaoAsync(filtro));
}
