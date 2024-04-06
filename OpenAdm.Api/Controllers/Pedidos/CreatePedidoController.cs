using Domain.Pkg.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CreatePedidoController : ControllerBaseApi
{
    private readonly ICreatePedidoService _createPedidoService;
    private readonly ITokenService _tokenService;
    private readonly IUsuarioRepository _usuarioRepository;

    public CreatePedidoController(
        ICreatePedidoService createPedidoService, 
        ITokenService tokenService, 
        IUsuarioRepository usuarioRepository)
    {
        _createPedidoService = createPedidoService;
        _tokenService = tokenService;
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePedido(IList<ItensPedidoModel> itensPedidoModels)
    {
        try
        {
            var id = _tokenService.GetTokenUsuarioViewModel().Id;
            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return Unauthorized();
            }
            var result = await _createPedidoService.CreatePedidoAsync(itensPedidoModels, usuario);

            return Ok(new { message = "Pedido criado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
