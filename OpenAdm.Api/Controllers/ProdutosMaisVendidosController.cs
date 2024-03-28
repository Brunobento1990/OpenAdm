using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("produtos-mais-vendidos")]
public class ProdutosMaisVendidosController : ControllerBaseApi
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
        try
        {
            var produtosMaisVendidos = await _produtosMaisVendidosService.GetProdutosMaisVendidosAsync();
            return Ok(produtosMaisVendidos);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
