using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("parcela")]
[IsFuncionario]
[AutenticaParceiro]
[Autentica]
public class ParcelaController : ControllerBase
{
    private readonly IParcelaService _parcelaService;

    public ParcelaController(IParcelaService parcelaService)
    {
        _parcelaService = parcelaService;
    }

    [HttpPost("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<FaturaViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Paginacao(PaginacaoParcelaDto paginacaoFaturaAReceberDto)
    {
        var paginacaoViewModel = await _parcelaService.PaginacaoAsync(paginacaoFaturaAReceberDto);
        return Ok(paginacaoViewModel);
    }

    [HttpGet("pedido")]
    [ProducesResponseType<IList<FaturaViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ByPedido([FromQuery] Guid pedidoId, [FromQuery] StatusParcelaEnum statusFatura)
    {
        var faturas = await _parcelaService.GetByPedidoIdAsync(pedidoId, statusFatura);
        return Ok(faturas);
    }

    [HttpGet("get-by-id")]
    [ProducesResponseType<FaturaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var fatura = await _parcelaService.GetByIdAsync(id);
        return Ok(fatura);
    }

    [HttpPut("pagar")]
    [ProducesResponseType<FaturaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Pagar(PagarParcelaDto pagarFaturaAReceberDto)
    {
        var fatura = await _parcelaService.PagarAsync(pagarFaturaAReceberDto);
        return Ok(fatura);
    }

    [HttpPut("edit")]
    [ProducesResponseType<FaturaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Edit(FaturaEdit faturaAReceberEdit)
    {
        var fatura = await _parcelaService.EditAsync(faturaAReceberEdit);
        return Ok(fatura);
    }
}
