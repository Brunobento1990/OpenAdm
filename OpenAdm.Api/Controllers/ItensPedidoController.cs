using Domain.Pkg.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("itens-pedidos")]
[Autentica]
public class ItensPedidoController : ControllerBase
{
    private readonly IItensPedidoService _itensPedidoService;

    public ItensPedidoController(IItensPedidoService itensPedidoService)
    {
        _itensPedidoService = itensPedidoService;
    }

    [HttpGet("get-pedido-id")]
    public async Task<IActionResult> GetByPedido([FromQuery] Guid pedidoId)
    {
        var itens = await _itensPedidoService.GetItensPedidoByPedidoIdAsync(pedidoId);
        return Ok(itens);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteItem([FromQuery] Guid id)
    {
        var result = await _itensPedidoService.DeleteItemPedidoAsync(id);
        if (!result)
        {
            return BadRequest(new { message = CodigoErrors.ErrorGeneric });
        }
        return Ok();
    }

    [HttpPut("update-quantidade")]
    public async Task<IActionResult> UpdateQuantidade(UpdateQuantidadeItemPedidoDto updateQuantidadeItemPedidoDto)
    {
        await _itensPedidoService.EditarQuantidadeDoItemAsync(updateQuantidadeItemPedidoDto);
        return Ok(new { message = "Item editado com sucesso!" });
    }
}
