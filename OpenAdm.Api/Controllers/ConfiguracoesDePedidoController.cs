using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ConfiguracoesDePedidos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracoes-de-pedido")]
[Autentica]
[AcessoParceiro]
public class ConfiguracoesDePedidoController : ControllerBase
{
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;

    public ConfiguracoesDePedidoController(
        IConfiguracoesDePedidoService configuracoesDePedidoService)
    {
        _configuracoesDePedidoService = configuracoesDePedidoService;
    }

    [IsFuncionario]
    [HttpGet("get-configuracoes")]
    public async Task<IActionResult> GetConfiguracoes()
    {
        var configuracoesDePedidoViewModel = await
            _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();

        return Ok(configuracoesDePedidoViewModel);
    }

    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateConfiguracao(UpdateConfiguracoesDePedidoDto updateConfiguracoesDePedidoDto)
    {
        var configuracaoDePedidoViewModel = await _configuracoesDePedidoService
            .CreateConfiguracoesDePedidoAsync(updateConfiguracoesDePedidoDto);

        return Ok(configuracaoDePedidoViewModel);
    }

    [HttpGet("pedido-minimo")]
    public async Task<IActionResult> GetPedidoMinimo()
    {
        var pedidoMinimoViewModl = await _configuracoesDePedidoService.GetPedidoMinimoAsync();
        return Ok(pedidoMinimoViewModl);
    }

    [HttpGet("todas-configuracoes")]
    [ProducesResponseType<TodasConfiguracoesViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> TodasConfiguracaoes()
    {
        var pedidoMinimoViewModl = await _configuracoesDePedidoService.GetTodasConfiguracoesAsync();
        return Ok(pedidoMinimoViewModl);
    }
}