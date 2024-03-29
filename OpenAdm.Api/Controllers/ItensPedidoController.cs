﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("itens-pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ItensPedidoController : ControllerBaseApi
{
    private readonly IItensPedidoService _itensPedidoService;

    public ItensPedidoController(IItensPedidoService itensPedidoService)
    {
        _itensPedidoService = itensPedidoService;
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("get-pedido-id")]
    public async Task<IActionResult> GetByPedido([FromQuery] Guid pedidoId)
    {
        try
        {
            var itens = await _itensPedidoService.GetItensPedidoByPedidoIdAsync(pedidoId);
            return Ok(itens);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
