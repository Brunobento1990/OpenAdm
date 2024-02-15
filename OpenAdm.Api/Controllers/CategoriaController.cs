using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("categorias")]
public class CategoriaController : ControllerBaseApi
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCategorias()
    {
        try
        {
            var categoriasViewModel = await _categoriaService.GetCategoriasAsync();
            return Ok(categoriasViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
