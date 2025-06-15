using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ConfiguracoesDeEmails;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracoes-de-email")]
[IsFuncionario]
[Autentica]
[AcessoParceiro]
public class ConfiguracoesDeEmailController : ControllerBase
{
    private readonly IConfiguracoesDeEmailService _configuracoesDeEmailService;

    public ConfiguracoesDeEmailController(IConfiguracoesDeEmailService configuracoesDeEmailService)
    {
        _configuracoesDeEmailService = configuracoesDeEmailService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateConfiguracoesDeEmail(CreateConfiguracoesDeEmailDto createConfiguracoesDeEmailDto)
    {
        var configuracoaDeEmailViewModel = await _configuracoesDeEmailService
            .CreateConfiguracoesDeEmailAsync(createConfiguracoesDeEmailDto);

        return Ok(configuracoaDeEmailViewModel);
    }

    [HttpGet("get-configuracao")]
    public async Task<IActionResult> GetConfiguracao()
    {
        var configuracaoDeEmailViewModel = await _configuracoesDeEmailService
            .GetConfiguracaoDeEmailAsync();

        return Ok(configuracaoDeEmailViewModel);
    }
}
