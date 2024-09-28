using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Autentica]
[AutenticaParceiro]
public class CoutCarrinhoController : ControllerBase
{
    private readonly IGetCountCarrinhoService _carrinhoService;
    private readonly Guid _usuarioId;

    public CoutCarrinhoController(IGetCountCarrinhoService carrinhoService,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _usuarioId = usuarioAutenticado.Id;
        _carrinhoService = carrinhoService;
    }

    [HttpGet("get-carrinho-count")]
    public async Task<IActionResult> GetCarrinhoCount()
    {
        var result = await _carrinhoService.GetCountCarrinhoAsync(_usuarioId);
        return Ok(result);
    }
}
