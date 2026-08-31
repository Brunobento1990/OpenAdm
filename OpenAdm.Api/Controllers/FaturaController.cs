using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.FaturasModel;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("fatura")]
[Autentica]
[IsFuncionario]
[AcessoParceiro]
public class FaturaController : ControllerBase
{
    private readonly IFaturaService _faturaService;

    public FaturaController(IFaturaService faturaService)
    {
        _faturaService = faturaService;
    }

    [HttpPost("criar")]
    [ProducesResponseType<PaginacaoViewModel<FaturaViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Criar(FaturaCriarAdmDto faturaCriarAdmDto)
    {
        var result = await _faturaService.CriarAdmAsync(faturaCriarAdmDto);
        return Ok(result);
    }

    [HttpPost("bonificar")]
    [ProducesResponseType<ResultadoPadraoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Bonificar(BaixaAutomaticaDto dto)
    {
        var result = await _faturaService.CriarBonificadaAsync(dto);

        return result.ToActionResult();
    }

    [HttpPost("bonificado/paginacao")]
    [ProducesResponseType<PaginacaoViewModel<FaturaBonificadaPaginacaoViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Paginacao(PaginacaoFaturaBonificadaDto dto)
    {
        var result = await _faturaService.PaginacaoBonificadasAsync(dto);
        return Ok(result);
    }

    [HttpPost("baixa-automatica")]
    [ProducesResponseType<ResultadoPadraoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> BaixaAutomatica(BaixaAutomaticaDto dto)
    {
        var result = await _faturaService.BaixaAutomaticaAsync(dto);
        return result.ToActionResult();
    }

    [HttpPost("negociar")]
    [ProducesResponseType<ResultadoPadraoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Negociar(NegociarCobrancaPedidoDto dto)
    {
        var result = await _faturaService.NegociarCobrancaAsync(dto);
        return result.ToActionResult();
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
