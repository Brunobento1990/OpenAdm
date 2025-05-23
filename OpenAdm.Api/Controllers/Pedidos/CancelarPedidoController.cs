using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[AcessoParceiro]
[Autentica]
public class CancelarPedidoController : ControllerBase
{
    private readonly ICancelarPedido _cancelarPedido;

    public CancelarPedidoController(ICancelarPedido cancelarPedido)
    {
        _cancelarPedido = cancelarPedido;
    }

    [HttpPut("cancelar")]
    public async Task<IActionResult> Cancelar(CancelarPedidoDto cancelarPedidoDto)
    {
        var result = await _cancelarPedido.CancelarAsync(cancelarPedidoDto);

        return Ok(new
        {
            result
        });
    }
}
