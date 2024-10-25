using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pagamento")]
[AutenticaParceiro]
[Autentica]
public class PagamentoController : ControllerBase
{
    private readonly IContasAReceberService _contasAReceberService;

    public PagamentoController(IContasAReceberService contasAReceberService)
    {
        _contasAReceberService = contasAReceberService;
    }

    [HttpPost("gerar-pagamento")]
    [ProducesResponseType<PagamentoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GerarPagamento(GerarPagamentoDto gerarPagamentoDto)
    {
        var result = await _contasAReceberService.GerarPagamentoAsync(gerarPagamentoDto.MeioDePagamento, gerarPagamentoDto.PedidoId);
        return Ok(result);
    }
}
