using Domain.Pkg.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Estoques;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
[IsFuncionario]
[Route("estoques")]
public class EstoqueController : ControllerBaseApi
{
    private readonly IEstoqueService _estoqueservice;

    public EstoqueController(IEstoqueService estoqueservice)
    {
        _estoqueservice = estoqueservice;
    }

    [HttpPut("movimentar-estoque")]
    public async Task<IActionResult> MovimentarEstoque(MovimentacaoDeProdutoDto movimentacaoDeProduto)
    {
        try
        {
            var result = await _estoqueservice.MovimentacaoDeProdutoAsync(movimentacaoDeProduto);

            if (!result) return BadRequest(new { message = CodigoErrors.ErrorGeneric });

            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoEstoqueDto paginacaoEstoqueDto)
    {
        try
        {
            var paginacao = await _estoqueservice.GetPaginacaoAsync(paginacaoEstoqueDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
