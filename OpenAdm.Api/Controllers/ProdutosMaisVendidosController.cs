using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("produtos-mais-vendidos")]
[AcessoParceiro]
public class ProdutosMaisVendidosController : ControllerBase
{
    private readonly IProdutosMaisVendidosService _produtosMaisVendidosService;

    public ProdutosMaisVendidosController(IProdutosMaisVendidosService produtosMaisVendidosService)
    {
        _produtosMaisVendidosService = produtosMaisVendidosService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var produtosMaisVendidos = await _produtosMaisVendidosService.GetProdutosMaisVendidosAsync();
        return Ok(produtosMaisVendidos);
    }
}
