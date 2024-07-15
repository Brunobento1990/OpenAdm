using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ConfiguracoesDeFrete;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDeFrete;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracao-de-frete")]
[Autentica]
[IsFuncionario]
public class ConfiguracaoDeFreteController : ControllerBase
{
    private readonly IConfiguracaoDeFreteService _configuracaoDeFreteService;

    public ConfiguracaoDeFreteController(IConfiguracaoDeFreteService configuracaoDeFreteService)
    {
        _configuracaoDeFreteService = configuracaoDeFreteService;
    }

    [HttpPost]
    [ProducesResponseType<ConfiguracaoDeFreteViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [ProducesResponseType<ErrorResponse>(401)]
    public async Task<IActionResult> CreateOrUpdate(ConfiguracaoDeFreteCreateDto configuracaoDeFreteCreateDto)
    {
        var response = await _configuracaoDeFreteService.CreateOrUpdateAsync(configuracaoDeFreteCreateDto);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType<ConfiguracaoDeFreteViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [ProducesResponseType<ErrorResponse>(401)]
    public async Task<IActionResult> Get()
    {
        var response = await _configuracaoDeFreteService.GetAsync();
        return Ok(response);
    }
}
