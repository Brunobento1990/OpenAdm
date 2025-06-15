using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ConfiguracoesDeFretes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDeFretes;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracao-de-frete")]
[AcessoParceiro]
public class ConfiguracaoDeFreteController : ControllerBase
{
    private readonly IConfiguracaoDeFreteService _configuracaoDeFreteService;

    public ConfiguracaoDeFreteController(IConfiguracaoDeFreteService configuracaoDeFreteService)
    {
        _configuracaoDeFreteService = configuracaoDeFreteService;
    }

    [HttpGet("cobrar")]
    [ProducesResponseType<EfetuarCobrancaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [Autentica]
    public async Task<IActionResult> Cobrar()
    {
        var result = await _configuracaoDeFreteService.CobrarFreteAsync();
        return Ok(result);
    }

    [HttpGet("get")]
    [ProducesResponseType<ConfiguracaoDeFreteViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [Autentica]
    [IsFuncionario]
    public async Task<IActionResult> Get()
    {
        var result = await _configuracaoDeFreteService.GetAsync();
        return Ok(result);
    }

    [HttpPost("create-or-update")]
    [ProducesResponseType<ConfiguracaoDeFreteViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [Autentica]
    [IsFuncionario]
    public async Task<IActionResult> CreateOrUpdate(ConfiguracaoDeFreteCreateOrUpdateDto configuracaoDeFreteCreateOrUpdateDto)
    {
        var result = await _configuracaoDeFreteService.CreateOrUpdateAsync(configuracaoDeFreteCreateOrUpdateDto);
        return Ok(result);
    }
}
