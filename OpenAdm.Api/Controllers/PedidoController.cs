using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
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
    [HttpPost("paginacao")]
    public async Task<IActionResult> Paginacao(PaginacaoPedidoDto paginacaoPedidoDto)
    {
        var paginacaoViewModel = await _pedidoService.GetPaginacaoAsync(paginacaoPedidoDto);
        return Ok(paginacaoViewModel);
    }

    [IsFuncionario]
    [HttpGet("list-em-aberto")]
    public async Task<IActionResult> PedidoEmAberto()
    {
        var paginacaoViewModel = await _pedidoService.GetPedidosEmAbertAsync();
        return Ok(paginacaoViewModel);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetPedidos([FromQuery] int statusPedido)
    {
        var pedidos = await _pedidoService.GetPedidosUsuarioAsync(statusPedido, _usuarioAutenticado.Id);
        return Ok(pedidos);
    }

    [HttpGet("get")]
    [ProducesResponseType<PedidoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Get(Guid pedidoId)
    {
        var pedido = await _pedidoService.GetAsync(pedidoId);
        return Ok(pedido);
    }

    [HttpGet("get-gerar-pix")]
    [ProducesResponseType<PedidoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GetGerarPix(Guid pedidoId)
    {
        var pedido = await _pedidoService.GetParaGerarPixAsync(pedidoId);
        return Ok(pedido);
    }
}
