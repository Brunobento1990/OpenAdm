using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Carrinhos;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Autentica]
[AutenticaParceiro]
public class CoutCarrinhoController : ControllerBase
{
    private readonly IGetCountCarrinhoService _carrinhoService;

    public CoutCarrinhoController(IGetCountCarrinhoService carrinhoService)
    {
        _carrinhoService = carrinhoService;
    }

    [HttpGet("get-carrinho-count")]
    public async Task<IActionResult> GetCarrinhoCount()
    {
        var result = await _carrinhoService.GetCountCarrinhoAsync();
        return Ok(result);
    }
}
