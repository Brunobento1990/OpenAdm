using Domain.Pkg.Model;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Autentica]
public class CreatePedidoController : ControllerBase
{
    private readonly ICreatePedidoService _createPedidoService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public CreatePedidoController(
        ICreatePedidoService createPedidoService,
        IUsuarioRepository usuarioRepository,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _createPedidoService = createPedidoService;
        _usuarioRepository = usuarioRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePedido(IList<ItensPedidoModel> itensPedidoModels)
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        var result = await _createPedidoService.CreatePedidoAsync(itensPedidoModels, usuario);

        return Ok(new { message = "Pedido criado com sucesso!", pedido = result });
    }
}
