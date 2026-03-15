using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracaoDeFreteModel;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracao-de-frete")]
public class ConfiguracaoDeFreteController : ControllerBase
{
    private readonly IConfiguracaoDeFreteService _configuracaoDeFreteService;

    public ConfiguracaoDeFreteController(IConfiguracaoDeFreteService configuracaoDeFreteService)
    {
        _configuracaoDeFreteService = configuracaoDeFreteService;
    }

    [HttpGet]
    [Autentica]
    [AcessoParceiro]
    [ProducesResponseType<ConfiguracaoDeFreteViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ObterAsync()
    {
        var result = await _configuracaoDeFreteService.ObterAsync();
        return result.ToActionResult();
    }

    [HttpPost]
    [Autentica]
    [AcessoParceiro]
    [ProducesResponseType<ConfiguracaoDeFreteViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CriarOuEditarAsync(ConfiguracaoDeFreteDTO configuracaoDeFrete)
    {
        var result = await _configuracaoDeFreteService.CrairOuEditarAsync(configuracaoDeFrete);
        return result.ToActionResult();
    }
}