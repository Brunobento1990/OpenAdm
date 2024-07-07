using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Autentica]
[IsFuncionario]
public class DeletePedidoController : ControllerBase
{
    private readonly IDeletePedidoService _deletePedidoService;

    public DeletePedidoController(IDeletePedidoService deletePedidoService)
    {
        _deletePedidoService = deletePedidoService;
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        await _deletePedidoService.DeletePedidoAsync(id);
        return Ok();
    }
}
