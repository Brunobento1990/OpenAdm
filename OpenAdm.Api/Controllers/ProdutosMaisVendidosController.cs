using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("produtos-mais-vendidos")]
public class ProdutosMaisVendidosController : ControllerBase
{
    private readonly IProdutosMaisVendidosService _produtosMaisVendidosService;

    public ProdutosMaisVendidosController(IProdutosMaisVendidosService produtosMaisVendidosService)
    {
        _produtosMaisVendidosService = produtosMaisVendidosService;
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var produtosMaisVendidos = await _produtosMaisVendidosService.GetProdutosMaisVendidosAsync();
        return Ok(produtosMaisVendidos);
    }
}
