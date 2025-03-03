using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("categorias")]
[AutenticaParceiro]
public class CategoriaController(ICategoriaService categoriaService)
    : ControllerBase
{
    private readonly ICategoriaService _categoriaService = categoriaService;

    [ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCategorias()
    {
        var categoriasViewModel = await _categoriaService.GetCategoriasAsync();
        return Ok(categoriasViewModel);
    }

    [HttpPost("create")]
    [IsFuncionario]
    [Autentica]
    public async Task<IActionResult> CreateCategoria(CategoriaCreateDto categoriaCreateDto)
    {
        var categoriaVieqModel = await _categoriaService.CreateCategoriaAsync(categoriaCreateDto);
        return Ok(categoriaVieqModel);
    }

    [HttpPost("paginacao")]
    [IsFuncionario]
    [Autentica]
    public async Task<IActionResult> PaginacaoCategoria(PaginacaoCategoriaDto paginacaoCategoriaDto)
    {
        var paginacao = await _categoriaService.GetPaginacaoAsync(paginacaoCategoriaDto);
        return Ok(paginacao);
    }

    [HttpGet("get-categoria")]
    [IsFuncionario]
    [Autentica]
    public async Task<IActionResult> GetCategoria([FromQuery] Guid id)
    {
        var categoriaViewModel = await _categoriaService.GetCategoriaAsync(id);
        return Ok(categoriaViewModel);
    }

    [HttpDelete("delete")]
    [IsFuncionario]
    [Autentica]
    public async Task<IActionResult> DeleteCategoria([FromQuery] Guid id)
    {
        await _categoriaService.DeleteCategoriaAsync(id);
        return Ok();
    }

    [HttpPut("update")]
    [IsFuncionario]
    [Autentica]
    public async Task<IActionResult> UpdateCategoria(UpdateCategoriaDto updateCategoriaDto)
    {
        var categoriaViewModel = await _categoriaService.UpdateCategoriaAsync(updateCategoriaDto);
        return Ok(categoriaViewModel);
    }

    [HttpPut("inativar-ativar")]
    [IsFuncionario]
    [Autentica]
    public async Task<IActionResult> InativarAtivar([FromQuery] Guid id)
    {
        await _categoriaService.InativarAtivarEcommerceAsync(id);
        return Ok(new
        {
            result = true
        });
    }
}
