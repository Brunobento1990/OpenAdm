using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Autentica]
[AcessoParceiro]
public class CreatePedidoController : ControllerBase
{
    private readonly ICreatePedidoService _createPedidoService;

    public CreatePedidoController(
        ICreatePedidoService createPedidoService)
    {
        _createPedidoService = createPedidoService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePedido(PedidoCreateDto pedidoCreateDto)
    {
        var result = await _createPedidoService.CreatePedidoAsync(pedidoCreateDto);

        return Ok(new { message = "Pedido criado com sucesso!", pedido = result });
    }
}
