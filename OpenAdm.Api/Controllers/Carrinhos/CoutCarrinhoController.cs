using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Carrinhos;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CoutCarrinhoController : ControllerBaseApi
{
    private readonly IGetCountCarrinhoService _carrinhoService;
    private readonly Guid _usuarioId;

    public CoutCarrinhoController(IGetCountCarrinhoService carrinhoService,
        ITokenService tokenService)
    {
        _usuarioId = tokenService.GetTokenUsuarioViewModel().Id;
        _carrinhoService = carrinhoService;
    }

    [HttpGet("get-carrinho-count")]
    public async Task<IActionResult> GetCarrinhoCount()
    {
        try
        {
            var result = await _carrinhoService.GetCountCarrinhoAsync(_usuarioId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
