using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ConfiguracoesDePagamentos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracao-de-pagamento")]
[AutenticaParceiro]
public class ConfiguracaoDePagamentoController : ControllerBase
{
    private readonly IConfiguracaoDePagamentoService _configuracaoDePagamentoService;

    public ConfiguracaoDePagamentoController(IConfiguracaoDePagamentoService configuracaoDePagamentoService)
    {
        _configuracaoDePagamentoService = configuracaoDePagamentoService;
    }

    [HttpGet("cobrar")]
    [Autentica]
    [ProducesResponseType<EfetuarCobrancaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Cobrar()
    {
        var result = await _configuracaoDePagamentoService.CobrarAsync();
        return Ok(result);
    }

    [HttpGet("get")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType<ConfiguracaoDePagamentoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Get()
    {
        var result = await _configuracaoDePagamentoService.GetAsync();
        return Ok(result);
    }

    [HttpPost("create-or-update")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType<ConfiguracaoDePagamentoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CreateOrUpdate(ConfiguracaoDePagamentoCreateOrUpdate configuracaoDePagamentoCreateOrUpdate)
    {
        var result = await _configuracaoDePagamentoService.CreateOrUpdateAsync(configuracaoDePagamentoCreateOrUpdate);
        return Ok(result);
    }
}
