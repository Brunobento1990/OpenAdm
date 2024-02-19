using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class PedidoController : ControllerBaseApi
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePedido(PedidoCreateDto pedidoCreateDto)
    {
        try
        {
            var result = await _pedidoService.CreatePedidoAsync(pedidoCreateDto);

            return Ok(new { message = "Pedido criado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
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

    [IsFuncionario]
    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateStatusPedido(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        try
        {
            await _pedidoService.UpdateStatusPedidoAsync(updateStatusPedidoDto);
            return Ok(new { message = "Pedido atualizado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [IsFuncionario]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        try
        {
            await _pedidoService.DeletePedidoAsync(id);
            return Ok();
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
            var pedidos = await _pedidoService.GetPedidosUsuarioAsync(statusPedido);
            return Ok(pedidos);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [IsFuncionario]
    [HttpGet("reenviar-pedido")]
    public async Task<IActionResult> ReenviarPedido([FromQuery] Guid pedidoId)
    {
        try
        {
            await _pedidoService.ReenviarPedidoViaEmailAsync(pedidoId);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
