﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ConfiguracoesDeEmails;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracoes-de-email")]
[IsFuncionario]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ConfiguracoesDeEmailController : ControllerBaseApi
{
    private readonly IConfiguracoesDeEmailService _configuracoesDeEmailService;

    public ConfiguracoesDeEmailController(IConfiguracoesDeEmailService configuracoesDeEmailService)
    {
        _configuracoesDeEmailService = configuracoesDeEmailService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateConfiguracoesDeEmail(CreateConfiguracoesDeEmailDto createConfiguracoesDeEmailDto)
    {
        try
        {
            var configuracoaDeEmailViewModel = await _configuracoesDeEmailService
                .CreateConfiguracoesDeEmailAsync(createConfiguracoesDeEmailDto);
            
            return Ok(configuracoaDeEmailViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
