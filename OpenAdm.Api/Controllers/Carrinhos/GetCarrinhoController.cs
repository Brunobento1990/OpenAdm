using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Carrinhos;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Autentica]
[AcessoParceiro]
public class GetCarrinhoController : ControllerBase
{
    private readonly IGetCarrinhoService _getCarrinhoService;

    public GetCarrinhoController(IGetCarrinhoService getCarrinhoService)
    {
        _getCarrinhoService = getCarrinhoService;
    }

    [HttpGet("get-carrinho")]
    public async Task<IActionResult> GetCarrinho()
    {
        var result = await _getCarrinhoService.GetCarrinhoAsync();
        return Ok(result);
    }
}
