using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("itens-pedidos")]
[Autentica]
[AutenticaParceiro]
public class ItensPedidoController : ControllerBase
{
    private readonly IItensPedidoService _itensPedidoService;
    private readonly IPedidoService _pedidoService;
    public ItensPedidoController(IItensPedidoService itensPedidoService, IPedidoService pedidoService)
    {
        _itensPedidoService = itensPedidoService;
        _pedidoService = pedidoService;
    }

    [HttpPost("producao/pdf")]
    public async Task<IActionResult> ProducaoPdf(RelatorioProducaoDto relatorioProducaoDto)
    {
        var pdf = await _pedidoService.PedidoProducaoAsync(relatorioProducaoDto);
        return Ok(new
        {
            pdf
        });
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
            return BadRequest(new { message = "Ocorreu um erro, tente novamente" });
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
