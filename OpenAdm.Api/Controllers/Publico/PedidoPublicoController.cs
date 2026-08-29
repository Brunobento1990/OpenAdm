using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces.Pedidos;

namespace OpenAdm.Api.Controllers.Publico;

[AllowAnonymous]
[ApiController]
[Route("publico/{empresaId:guid}/pedido/{idPublico:guid}")]
public sealed class PedidoPublicoController : ControllerBase
{
    private readonly IPedidoPublicoService _pedidoPublicoService;

    public PedidoPublicoController(IPedidoPublicoService pedidoPublicoService)
    {
        _pedidoPublicoService = pedidoPublicoService;
    }

    [HttpGet("pdf")]
    public async Task<IActionResult> Pdf(Guid empresaId, Guid idPublico)
    {
        var pdf = await _pedidoPublicoService.GerarPdfAsync(empresaId, idPublico);
        return pdf == null
            ? NotFound()
            : File(pdf, "application/pdf", $"pedido-{idPublico:N}.pdf");
    }
}
