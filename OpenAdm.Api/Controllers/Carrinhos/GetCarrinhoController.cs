using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Carrinhos;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class GetCarrinhoController : ControllerBaseApi
{
    private readonly IGetCarrinhoService _getCarrinhoService;
    private readonly ITokenService _tokenService;

    public GetCarrinhoController(IGetCarrinhoService getCarrinhoService, ITokenService tokenService)
    {
        _getCarrinhoService = getCarrinhoService;
        _tokenService = tokenService;
    }

    [HttpGet("get-carrinho")]
    public async Task<IActionResult> GetCarrinho()
    {
        try
        {
            var usuarioViewModel = _tokenService.GetTokenUsuarioViewModel();
            var result = await _getCarrinhoService.GetCarrinhoAsync(usuarioViewModel);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
