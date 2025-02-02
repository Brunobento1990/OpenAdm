using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Model;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("fatura")]
[Autentica]
[IsFuncionario]
[AutenticaParceiro]
public class FaturaController : ControllerBase
{
    private readonly IFaturaService _faturaService;
    private readonly IGerarPixPedidoService _gerarPixPedidoService;
    public FaturaController(IFaturaService faturaService, IGerarPixPedidoService gerarPixPedidoService)
    {
        _faturaService = faturaService;
        _gerarPixPedidoService = gerarPixPedidoService;
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

    [HttpPost("gerar-pix")]
    [ProducesResponseType<PagamentoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GerarPix(GerarPixParcelaDto gerarPixParcelaDto)
    {
        var result = await _gerarPixPedidoService.GerarPixAsync(gerarPixParcelaDto);
        return Ok(result);
    }
}
