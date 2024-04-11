using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.ConfiguracoesDePedidos;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("configuracoes-de-pedido")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ConfiguracoesDePedidoController : ControllerBaseApi
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
        try
        {
            var configuracoesDePedidoViewModel = await
                _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();

            return Ok(configuracoesDePedidoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateConfiguracao(UpdateConfiguracoesDePedidoDto updateConfiguracoesDePedidoDto)
    {
        try
        {
            var configuracaoDePedidoViewModel = await _configuracoesDePedidoService
                .CreateConfiguracoesDePedidoAsync(updateConfiguracoesDePedidoDto);

            return Ok(configuracaoDePedidoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("pedido-minimo")]
    public async Task<IActionResult> GetPedidoMinimo()
    {
        try
        {
            var pedidoMinimoViewModl = await _configuracoesDePedidoService.GetPedidoMinimoAsync();
            return Ok(pedidoMinimoViewModl);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
