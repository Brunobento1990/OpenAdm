using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("categorias")]
[AcessoParceiro]
public class CategoriaController(ICategoriaService categoriaService)
    : ControllerBase
{
    private readonly ICategoriaService _categoriaService = categoriaService;

    [HttpGet("list")]
    [ProducesResponseType<IList<CategoriaViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GetCategorias()
    {
        var categoriasViewModel = await _categoriaService.GetCategoriasAsync();
        return Ok(categoriasViewModel);
    }

    [HttpGet("list-drop-down")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType<IList<CategoriaViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GetCategoriasDropDown()
    {
        var categoriasViewModel = await _categoriaService.GetCategoriasAsync();
        return Ok(categoriasViewModel);
    }

    [HttpPost("create")]
    [Autentica]
    [IsFuncionario]
    [ProducesResponseType<CategoriaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CreateCategoria(CategoriaCreateDto categoriaCreateDto)
    {
        var categoriaVieqModel = await _categoriaService.CreateCategoriaAsync(categoriaCreateDto);
        return Ok(categoriaVieqModel);
    }

    [HttpPost("paginacao")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType<PaginacaoViewModel<CategoriaViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> PaginacaoCategoria(PaginacaoCategoriaDto paginacaoCategoriaDto)
    {
        var paginacao = await _categoriaService.GetPaginacaoAsync(paginacaoCategoriaDto);
        return Ok(paginacao);
    }

    [HttpGet("get-categoria")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType<CategoriaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> GetCategoria([FromQuery] Guid id)
    {
        var categoriaViewModel = await _categoriaService.GetCategoriaAsync(id);
        return Ok(categoriaViewModel);
    }

    [HttpDelete("delete")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> DeleteCategoria([FromQuery] Guid id)
    {
        await _categoriaService.DeleteCategoriaAsync(id);
        return Ok();
    }

    [HttpPut("update")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType<CategoriaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> UpdateCategoria(UpdateCategoriaDto updateCategoriaDto)
    {
        var categoriaViewModel = await _categoriaService.UpdateCategoriaAsync(updateCategoriaDto);
        return Ok(categoriaViewModel);
    }

    [HttpPut("inativar-ativar")]
    [IsFuncionario]
    [Autentica]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> InativarAtivar([FromQuery] Guid id)
    {
        await _categoriaService.InativarAtivarEcommerceAsync(id);
        return Ok(new
        {
            result = true
        });
    }
}
