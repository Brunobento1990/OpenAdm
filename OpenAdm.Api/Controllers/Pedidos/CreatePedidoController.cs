using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
public class CreatePedidoController : ControllerBase
{
    private readonly ICreatePedidoService _createPedidoService;

    public CreatePedidoController(
        ICreatePedidoService createPedidoService)
    {
        _createPedidoService = createPedidoService;
    }

    [HttpPost("create")]
    [Autentica]
    [AcessoParceiro]
    [ProducesResponseType<CriarPedidoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CreatePedido(PedidoCreateDto pedidoCreateDto)
    {
        var result = await _createPedidoService.CreatePedidoAsync(pedidoCreateDto);

        return result.ToActionResult();
    }
}