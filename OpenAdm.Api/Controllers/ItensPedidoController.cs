using Domain.Pkg.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("itens-pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ItensPedidoController : ControllerBaseApi
{
    private readonly IItensPedidoService _itensPedidoService;

    public ItensPedidoController(IItensPedidoService itensPedidoService)
    {
        _itensPedidoService = itensPedidoService;
    }

    [HttpGet("get-pedido-id")]
    public async Task<IActionResult> GetByPedido([FromQuery] Guid pedidoId)
    {
        try
        {
            var itens = await _itensPedidoService.GetItensPedidoByPedidoIdAsync(pedidoId);
            return Ok(itens);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteItem([FromQuery] Guid id)
    {
        try
        {
            var result = await _itensPedidoService.DeleteItemPedidoAsync(id);
            if (!result)
            {
                return BadRequest(new { message = CodigoErrors.ErrorGeneric });
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpPut("update-quantidade")]
    public async Task<IActionResult> UpdateQuantidade(UpdateQuantidadeItemPedidoDto updateQuantidadeItemPedidoDto)
    {
        try
        {
            await _itensPedidoService.EditarQuantidadeDoItemAsync(updateQuantidadeItemPedidoDto);
            return Ok(new { message = "Item editado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
