using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("fatura-contas-a-receber")]
[Autentica]
[IsFuncionario]
[AutenticaParceiro]
public class FaturaContasAReceberController : ControllerBase
{
    private readonly IFaturaContasAReceberService _faturaContasAReceberService;

    public FaturaContasAReceberController(IFaturaContasAReceberService faturaContasAReceberService)
    {
        _faturaContasAReceberService = faturaContasAReceberService;
    }

    [HttpGet("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<FaturaContasAReceberViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoFaturaAReceberDto paginacaoFaturaAReceberDto)
    {
        var paginacaoViewModel = await _faturaContasAReceberService.PaginacaoAsync(paginacaoFaturaAReceberDto);
        return Ok(paginacaoViewModel);
    }

    [HttpGet("pedido")]
    [ProducesResponseType<IList<FaturaContasAReceberViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ByPedido([FromQuery] Guid pedidoId, [FromQuery] StatusFaturaContasAReceberEnum statusFatura)
    {
        var faturas = await _faturaContasAReceberService.GetByPedidoIdAsync(pedidoId, statusFatura);
        return Ok(faturas);
    }

    [HttpPut("pagar")]
    [ProducesResponseType<FaturaContasAReceberViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Pagar(PagarFaturaAReceberDto pagarFaturaAReceberDto)
    {
        var fatura = await _faturaContasAReceberService.PagarAsync(pagarFaturaAReceberDto);
        return Ok(fatura);
    }
}
