using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
[IsFuncionario]
public class DownloadPedidoController : ControllerBaseApi
{
    private readonly IPedidoDownloadService _pedidoDownloadService;

    public DownloadPedidoController(IPedidoDownloadService pedidoDownloadService)
    {
        _pedidoDownloadService = pedidoDownloadService;
    }

    [HttpGet("download-pedido")]
    public async Task<IActionResult> DownloadPedido([FromQuery] Guid pedidoId)
    {
        try
        {
            var pdf = await _pedidoDownloadService.DownloadPedidoPdfAsync(pedidoId);
            return Ok(new { pdf });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
