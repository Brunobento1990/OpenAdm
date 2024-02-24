using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("categorias")]
public class CategoriaController(ICategoriaService categoriaService)
    : ControllerBaseApi
{
    private readonly ICategoriaService _categoriaService = categoriaService;

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
    public async Task<IActionResult> PaginacaoCategoria([FromQuery] PaginacaoCategoriaDto paginacaoCategoriaDto)
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

    [HttpGet("get-categoria")]
    [IsFuncionario]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetCategoria([FromQuery] Guid id)
    {
        try
        {
            var categoriaViewModel = await _categoriaService.GetCategoriaAsync(id);
            return Ok(categoriaViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpDelete("delete")]
    [IsFuncionario]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteCategoria([FromQuery] Guid id)
    {
        try
        {
            await _categoriaService.DeleteCategoriaAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpPut("update")]
    [IsFuncionario]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UpdateCategoria(UpdateCategoriaDto updateCategoriaDto)
    {
        try
        {
            var categoriaViewModel = await _categoriaService.UpdateCategoriaAsync(updateCategoriaDto);
            return Ok(categoriaViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
