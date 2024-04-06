using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class PedidoController : ControllerBaseApi
{
    private readonly IPedidoService _pedidoService;
    private readonly ITokenService _tokenService;

    public PedidoController(
        IPedidoService pedidoService,
        ITokenService tokenService)
    {
        _pedidoService = pedidoService;
        _tokenService = tokenService;
    }

    [IsFuncionario]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoPedidoDto paginacaoPedidoDto)
    {
        try
        {
            var paginacaoViewModel = await _pedidoService.GetPaginacaoAsync(paginacaoPedidoDto);
            return Ok(paginacaoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetPedidos([FromQuery] int statusPedido)
    {
        try
        {
            var usuarioId = _tokenService.GetTokenUsuarioViewModel().Id;
            var pedidos = await _pedidoService.GetPedidosUsuarioAsync(statusPedido, usuarioId);
            return Ok(pedidos);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
