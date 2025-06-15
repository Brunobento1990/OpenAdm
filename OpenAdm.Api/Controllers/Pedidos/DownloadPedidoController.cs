using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Autentica]
[IsFuncionario]
[AcessoParceiro]
public class DownloadPedidoController : ControllerBase
{
    private readonly IPedidoDownloadService _pedidoDownloadService;

    public DownloadPedidoController(IPedidoDownloadService pedidoDownloadService)
    {
        _pedidoDownloadService = pedidoDownloadService;
    }

    [HttpGet("download-pedido")]
    public async Task<IActionResult> DownloadPedido([FromQuery] Guid pedidoId)
    {
        var pdf = await _pedidoDownloadService.DownloadPedidoPdfAsync(pedidoId);
        return Ok(new { pdf });
    }
}
