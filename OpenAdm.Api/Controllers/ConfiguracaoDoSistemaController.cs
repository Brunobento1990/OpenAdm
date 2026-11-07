using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ConfiguracaoDoSistema;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracaoDoSistema;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracao-do-sistema")]
[Autentica]
[AcessoParceiro]
public class ConfiguracaoDoSistemaController : ControllerBase
{
    private readonly IConfiguracaoDoSistemaService _configuracaoDoSistemaService;

    public ConfiguracaoDoSistemaController(
        IConfiguracaoDoSistemaService configuracaoDoSistemaService)
    {
        _configuracaoDoSistemaService = configuracaoDoSistemaService;
    }

    [IsFuncionario]
    [HttpGet("get-configuracoes")]
    public async Task<IActionResult> GetConfiguracoes()
    {
        var configuracaoDoSistemaViewModel = await
            _configuracaoDoSistemaService.GetConfiguracaoDoSistemaAsync();

        return Ok(configuracaoDoSistemaViewModel);
    }

    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateConfiguracao(UpdateConfiguracaoDoSistemaDto updateConfiguracaoDoSistemaDto)
    {
        var configuracaoDoSistemaViewModel = await _configuracaoDoSistemaService
            .CreateConfiguracaoDoSistemaAsync(updateConfiguracaoDoSistemaDto);

        return Ok(configuracaoDoSistemaViewModel);
    }

    [HttpGet("pedido-minimo")]
    public async Task<IActionResult> GetPedidoMinimo()
    {
        var pedidoMinimoViewModl = await _configuracaoDoSistemaService.GetPedidoMinimoAsync();
        return Ok(pedidoMinimoViewModl);
    }

    [HttpGet("todas-configuracoes")]
    [ProducesResponseType<TodasConfiguracoesViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> TodasConfiguracaoes()
    {
        var pedidoMinimoViewModl = await _configuracaoDoSistemaService.GetTodasConfiguracoesAsync();
        return Ok(pedidoMinimoViewModl);
    }
}
