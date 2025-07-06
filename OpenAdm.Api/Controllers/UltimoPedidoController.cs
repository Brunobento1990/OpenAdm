using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("ultimos-pedidos")]
[AcessoParceiro]
[Autentica]
[IsFuncionario]
public class UltimoPedidoController : ControllerBase
{
    private readonly IUsuarioPedidoServico _usuarioPedidoServico;

    public UltimoPedidoController(IUsuarioPedidoServico usuarioPedidoServico)
    {
        _usuarioPedidoServico = usuarioPedidoServico;
    }

    [HttpGet]
    [ProducesResponseType<PaginacaoUltimoPedidoUsuarioViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ListarUltimosPedidos([FromQuery] PaginacaoUltimoPedidoUsuarioDto paginacaoUltimoPedidoUsuarioDto)
    {
        var response = await _usuarioPedidoServico.ListarAsync(paginacaoUltimoPedidoUsuarioDto);
        return Ok(response);
    }
}
