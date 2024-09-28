using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pedidos")]
[Autentica]
[AutenticaParceiro]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public PedidoController(
        IPedidoService pedidoService,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _pedidoService = pedidoService;
        _usuarioAutenticado = usuarioAutenticado;
    }

    [IsFuncionario]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoPedidoDto paginacaoPedidoDto)
    {
        var paginacaoViewModel = await _pedidoService.GetPaginacaoAsync(paginacaoPedidoDto);
        return Ok(paginacaoViewModel);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetPedidos([FromQuery] int statusPedido)
    {
        var pedidos = await _pedidoService.GetPedidosUsuarioAsync(statusPedido, _usuarioAutenticado.Id);
        return Ok(pedidos);
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery] Guid pedidoId)
    {
        var pedido = await _pedidoService.GetAsync(pedidoId);
        return Ok(pedido);
    }
}
