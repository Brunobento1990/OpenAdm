using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using System.Text.Json;

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
    public async Task<IActionResult> PagamentoWebHook([FromBody] NotificationFaturaWebHook body)
    {
        Console.WriteLine("111");
        if (body?.Data != null && (body?.Action == "payment.update" || body?.Action == "payment.updated"))
        {
            if (!string.IsNullOrWhiteSpace(body.Data.Id))
            {
                Console.WriteLine("Processando pagamento!");
                await _faturaContasAReceberService.BaixarFaturaWebHookAsync(body);
                Console.WriteLine("Processamento concluído com sucesso!");
            }
            else
            {
                Console.WriteLine("Não ha ID no Data");
            }
        }
        else
        {
            Console.WriteLine($"Não achou o body: {JsonSerializer.Serialize(body)}");
        }

        return Ok();
    }
}
