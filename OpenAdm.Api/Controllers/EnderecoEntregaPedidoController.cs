using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.EnderecosDeEntregasPedidos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.EnderecosEntregasPedido;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("endereco-entrega-pedido")]
[AutenticaParceiro]
[Autentica]
public class EnderecoEntregaPedidoController : ControllerBase
{
    private readonly IEnderecoEntregaPedidoService _enderecoEntregaPedidoService;

    public EnderecoEntregaPedidoController(IEnderecoEntregaPedidoService enderecoEntregaPedidoService)
    {
        _enderecoEntregaPedidoService = enderecoEntregaPedidoService;
    }

    [HttpPost("create")]
    [ProducesResponseType<EnderecoEntregaPedidoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Create(EnderecoEntregaPedidoCreateDto enderecoEntregaPedidoCreateDto)
    {
        var result = await _enderecoEntregaPedidoService.CreateAsync(enderecoEntregaPedidoCreateDto);
        return Ok(result);
    }
}
