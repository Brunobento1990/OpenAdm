using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers.Representante;

[ApiController]
[Route("representante/pedidos")]
[AcessoParceiro]
[Autentica]
[IsRepresentante]
public sealed class PedidoRepresentanteController(
    IPedidoService pedidoService,
    IPedidoDownloadService pedidoDownloadService,
    IUsuarioAutenticado usuarioAutenticado,
    ICreatePedidoAdmService createPedidoAdmService) : ControllerBase
{
    [HttpGet("get")]
    [IsRepresentante]
    [ProducesResponseType<PedidoViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterPorIdAsync(Guid pedidoId) =>
        Ok(await pedidoService.GetAsync(pedidoId));

    [HttpGet("download-pedido")]
    [IsRepresentante]
    [Produces("application/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DownloadAsync([FromQuery] Guid pedidoId)
    {
        var (pdf, numeroPedido) = await pedidoDownloadService.DownloadPedidoPdfAsync(pedidoId);
        return File(pdf, "application/pdf", $"pedido-{numeroPedido}.pdf");
    }

    [HttpPost("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<PedidoViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PaginarAsync(PaginacaoPedidoDto filtro)
    {
        filtro.RepresentanteId = usuarioAutenticado.Id;
        return Ok(await pedidoService.GetPaginacaoAsync(filtro));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePedido(PedidoAdmCreateDto pedidoAdmCreateDto)
    {
        var result =
            await createPedidoAdmService.CreateAsync(pedidoAdmCreateDto, representanteId: usuarioAutenticado.Id,
                processarPedido: true);

        return Ok(new { result });
    }
}