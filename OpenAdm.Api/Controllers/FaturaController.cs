using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Model;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("fatura")]
[Autentica]
[IsFuncionario]
[AcessoParceiro]
public class FaturaController : ControllerBase
{
    private readonly IFaturaService _faturaService;
    private readonly IGerarCobrancaPedidoService _gerarCobrancaPedidoService;

    public FaturaController(IFaturaService faturaService, IGerarCobrancaPedidoService gerarCobrancaPedidoService)
    {
        _faturaService = faturaService;
        _gerarCobrancaPedidoService = gerarCobrancaPedidoService;
    }

    [HttpPost("criar")]
    [ProducesResponseType<PaginacaoViewModel<FaturaViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Criar(FaturaCriarAdmDto faturaCriarAdmDto)
    {
        var result = await _faturaService.CriarAdmAsync(faturaCriarAdmDto);
        return Ok(result);
    }

    [HttpGet("get")]
    [ProducesResponseType<FaturaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Get([FromQuery] Guid id)
    {
        var result = await _faturaService.GetCompletaAsync(id);
        return Ok(result);
    }
}