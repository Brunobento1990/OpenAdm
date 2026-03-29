using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;

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

    [HttpPost("cobrar")]
    [ProducesResponseType<PagamentoViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CobrarAsync([FromBody] GerarCobrancaPedidoDto gerarCobrancaPedidoDto)
    {
        var resultado = await _cobrancaPedidoService.CobrarAsync(gerarCobrancaPedidoDto);
        return resultado.ToActionResult();
    }
}