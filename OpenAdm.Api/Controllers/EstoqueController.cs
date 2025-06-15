using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Estoques;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("estoques")]
[Autentica]
[IsFuncionario]
[AcessoParceiro]
public class EstoqueController : ControllerBase
{
    private readonly IEstoqueService _estoqueservice;

    public EstoqueController(IEstoqueService estoqueservice)
    {
        _estoqueservice = estoqueservice;
    }

    [HttpPut("movimentar-estoque")]
    public async Task<IActionResult> MovimentarEstoque(MovimentacaoDeProdutoDto movimentacaoDeProduto)
    {
        var result = await _estoqueservice.MovimentacaoDeProdutoAsync(movimentacaoDeProduto);

        if (!result) return BadRequest(new { message = "Ocorreu um erro, tente novamente" });

        return Ok(new
        {
            resultado = true
        });
    }

    [HttpGet("get-estoque")]
    public async Task<IActionResult> GetEstoque([FromQuery] Guid id)
    {
        var estoqueViewModel = await _estoqueservice.GetEstoqueViewModelAsync(id);
        return Ok(estoqueViewModel);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEstoque(UpdateEstoqueDto updateEstoqueDto)
    {
        var result = await _estoqueservice.UpdateEstoqueAsync(updateEstoqueDto);

        if (!result) return BadRequest(new { message = "Ocorreu um erro, tente novamente" });

        return Ok(new
        {
            result
        });
    }

    [HttpPost("paginacao")]
    public async Task<IActionResult> Paginacao(PaginacaoEstoqueDto paginacaoEstoqueDto)
    {
        var paginacao = await _estoqueservice.GetPaginacaoAsync(paginacaoEstoqueDto);
        return Ok(paginacao);
    }
}
