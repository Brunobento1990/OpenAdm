using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.PaginateDto;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("categorias")]
public class CategoriaController(ICategoriaService categoriaService) 
    : ControllerBaseApi
{
    private readonly ICategoriaService _categoriaService = categoriaService;

    [HttpGet("list")]
    [ResponseCache(CacheProfileName = "Defautl60")]
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

    [HttpPost("create")]
    [IsFuncionario]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> CreateCategoria(CategoriaCreateDto categoriaCreateDto)
    {
        try
        {
            var categoriaVieqModel = await _categoriaService.CreateCategoriaAsync(categoriaCreateDto);
            return Ok(categoriaVieqModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("paginacao")]
    [IsFuncionario]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> PaginacaoCategoria([FromQuery]PaginacaoCategoriaDto paginacaoCategoriaDto)
    {
        try
        {
            var paginacao = await _categoriaService.GetPaginacaoAsync(paginacaoCategoriaDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
