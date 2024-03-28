using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
[IsFuncionario]
public class DeletePedidoController : ControllerBaseApi
{
    private readonly IDeletePedidoService _deletePedidoService;

    public DeletePedidoController(IDeletePedidoService deletePedidoService)
    {
        _deletePedidoService = deletePedidoService;
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        try
        {
            await _deletePedidoService.DeletePedidoAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
