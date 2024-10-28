using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;
using System.Text.Json;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pagamento")]

public class PagamentoController : ControllerBase
{
    private readonly IFaturaService _contasAReceberService;
    private readonly IParcelaService _faturaContasAReceberService;
    public PagamentoController(
        IFaturaService contasAReceberService,
        IParcelaService faturaContasAReceberService)
    {
        _contasAReceberService = contasAReceberService;
        _faturaContasAReceberService = faturaContasAReceberService;
    }

    [HttpPost("gerar-pagamento")]
    [AutenticaParceiro]
    [Autentica]
    [ProducesResponseType<PagamentoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GerarPagamento(GerarPagamentoDto gerarPagamentoDto)
    {
        var result = await _contasAReceberService.GerarPagamentoAsync(gerarPagamentoDto.MeioDePagamento, gerarPagamentoDto.PedidoId);
        return Ok(result);
    }

    [HttpPost("pagamento/notificar")]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [AutenticaMercadoPago]
    public async Task<IActionResult> PagamentoWebHook([FromBody] NotificationFaturaWebHook body)
    {
        if (body?.Data != null && (body?.Action == "payment.update" || body?.Action == "payment.updated"))
        {
            if (!string.IsNullOrWhiteSpace(body.Data.Id))
            {
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
