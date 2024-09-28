using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Autentica]
[AutenticaParceiro]
public class CreatePedidoController : ControllerBase
{
    private readonly ICreatePedidoService _createPedidoService;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public CreatePedidoController(
        ICreatePedidoService createPedidoService,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _createPedidoService = createPedidoService;
        _usuarioAutenticado = usuarioAutenticado;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePedido(IList<ItemPedidoModel> itensPedidoModels)
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        var result = await _createPedidoService.CreatePedidoAsync(itensPedidoModels, usuario);

        return Ok(new { message = "Pedido criado com sucesso!", pedido = result });
    }
}
