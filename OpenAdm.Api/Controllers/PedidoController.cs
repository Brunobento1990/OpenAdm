using Domain.Pkg.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class PedidoController : ControllerBaseApi
{
    private readonly IPedidoService _pedidoService;
    private readonly ITokenService _tokenService;
    private readonly IProcessarPedidoService _processarPedidoService;
    private readonly IUsuarioRepository _usuarioRepository;

    public PedidoController(
        IPedidoService pedidoService,
        ITokenService tokenService,
        IProcessarPedidoService processarPedidoService,
        IUsuarioRepository usuarioRepository)
    {
        _pedidoService = pedidoService;
        _tokenService = tokenService;
        _processarPedidoService = processarPedidoService;
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePedido(IList<ItensPedidoModel> itensPedidoModels)
    {
        try
        {
            var id = _tokenService.GetTokenUsuarioViewModel().Id;
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if(usuario == null)
            {
                return Unauthorized();
            }
            var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
            var result = await _pedidoService.CreatePedidoAsync(itensPedidoModels, usuarioViewModel);

            return Ok(new { message = "Pedido criado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [IsFuncionario]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoPedidoDto paginacaoPedidoDto)
    {
        try
        {
            var paginacaoViewModel = await _pedidoService.GetPaginacaoAsync(paginacaoPedidoDto);
            return Ok(paginacaoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [IsFuncionario]
    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateStatusPedido(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        try
        {
            await _pedidoService.UpdateStatusPedidoAsync(updateStatusPedidoDto);
            return Ok(new { message = "Pedido atualizado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [IsFuncionario]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        try
        {
            await _pedidoService.DeletePedidoAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPedidos([FromQuery] int statusPedido)
    {
        try
        {
            var usuarioId = _tokenService.GetTokenUsuarioViewModel().Id;
            var pedidos = await _pedidoService.GetPedidosUsuarioAsync(statusPedido, usuarioId);
            return Ok(pedidos);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [IsFuncionario]
    [HttpGet("reenviar-pedido")]
    public async Task<IActionResult> ReenviarPedido([FromQuery] Guid pedidoId)
    {
        try
        {
            await _pedidoService.ReenviarPedidoViaEmailAsync(pedidoId);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [IsFuncionario]
    [HttpGet("download-pedido")]
    public async Task<IActionResult> DownloadPedido([FromQuery] Guid pedidoId)
    {
        try
        {
            var pdf = await _processarPedidoService.DownloadPedidoPdfAsync(pedidoId);
            return Ok(new { pdf });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
