using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pagamento")]
public class PagamentoController : ControllerBase
{
    private readonly IParcelaService _faturaContasAReceberService;

    public PagamentoController(
        IParcelaService faturaContasAReceberService)
    {
        _faturaContasAReceberService = faturaContasAReceberService;
    }

    [HttpPost("notificar")]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [AutenticaMercadoPago]
    public async Task<IActionResult> PagamentoWebHook([FromBody] NotificationFaturaWebHook body,
        [FromQuery] Guid parceiroId)
    {
        if (body?.Data != null && (body?.Action == "payment.update" || body?.Action == "payment.updated") &&
            !string.IsNullOrWhiteSpace(body.Data.Id))
        {
            await _faturaContasAReceberService.BaixarFaturaWebHookAsync(body);
        }

        return Ok();
    }
}