using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos-adm")]
[Autentica]
[AcessoParceiro]
[IsFuncionario]
public class CreatePedidoAdmController : ControllerBase
{
    private readonly ICreatePedidoAdmService _createPedidoAdmService;

    public CreatePedidoAdmController(ICreatePedidoAdmService createPedidoAdmService)
    {
        _createPedidoAdmService = createPedidoAdmService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePedido(PedidoAdmCreateDto pedidoAdmCreateDto)
    {
        var result = await _createPedidoAdmService.CreateAsync(pedidoAdmCreateDto);

        return Ok(new { result });
    }
}
