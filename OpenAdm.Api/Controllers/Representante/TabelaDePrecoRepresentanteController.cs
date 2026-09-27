using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.TabelaDePrecos;
using OpenAdm.Application.Models.Representantes;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers.Representante;

[ApiController]
[Route("representante/tabelas-de-precos")]
[AcessoParceiro]
[Autentica]
[IsRepresentante]
public sealed class TabelaDePrecoRepresentanteController(
    ITabelaDePrecoService tabelaDePrecoService,
    IItemTabelaDePrecoService itemTabelaDePrecoService) : ControllerBase
{
    [HttpGet("get-tabela-ativa")]
    [ProducesResponseType<TabelaDePrecoViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterTabelaAtivaAsync() =>
        Ok(await tabelaDePrecoService.GetTabelaDePrecoViewModelAtivaAsync());

    [HttpPost("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<TabelaDePrecoViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PaginarAsync(PaginacaoTabelaDePrecoDto filtro) =>
        Ok(await tabelaDePrecoService.GetPaginacaoTabelaViewModelAsync(filtro));

    [HttpPost("itens/paginacao")]
    [ProducesResponseType<PaginacaoViewModel<ItemCatalogoRepresentanteViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PaginarItensAsync(PaginacaoItemCatalogoRepresentanteDto filtro) =>
        Ok(await itemTabelaDePrecoService.PaginarCatalogoRepresentanteAsync(filtro));
}
