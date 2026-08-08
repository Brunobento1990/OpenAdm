using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Application.Models.CobrancasPedidosEcommerce;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pedido")]
[AcessoParceiro]
[Autentica]
public class GerarCobrancaPedidoController : ControllerBase
{
    private readonly ICobrancaPedidoService _cobrancaPedidoService;

    public GerarCobrancaPedidoController(ICobrancaPedidoService cobrancaPedidoService)
    {
        _cobrancaPedidoService = cobrancaPedidoService;
    }

    [IsFuncionario]
    [HttpGet("cobranca")]
    [ProducesResponseType<CobrancaPedidoViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetParaNegociacaoAsync([FromQuery] Guid pedidoId)
    {
        var resultado = await _cobrancaPedidoService.GetParaNegociacaoAsync(pedidoId);
        return resultado.ToActionResult();
    }

    [HttpPost("cobrar")]
    [ProducesResponseType<PagamentoViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CobrarAsync([FromBody] GerarCobrancaPedidoDto gerarCobrancaPedidoDto)
    {
        var resultado = await _cobrancaPedidoService.CobrarAsync(gerarCobrancaPedidoDto);
        return resultado.ToActionResult();
    }
}
