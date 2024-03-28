using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
[IsFuncionario]
public class UpdateStatusPedidoController : ControllerBaseApi
{
    private readonly IUpdateStatusPedidoService _updateStatusPedidoService;

    public UpdateStatusPedidoController(IUpdateStatusPedidoService updateStatusPedidoService)
    {
        _updateStatusPedidoService = updateStatusPedidoService;
    }

    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateStatusPedido(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        try
        {
            await _updateStatusPedidoService.UpdateStatusPedidoAsync(updateStatusPedidoDto);
            return Ok(new { message = "Pedido atualizado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
