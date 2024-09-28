using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("movimentacao-de-produto")]
[Autentica]
[IsFuncionario]
[AutenticaParceiro]
public class MovimentacaoDeProdutoController : ControllerBase
{
    private readonly IMovimentacaoDeProdutosService _movimentacaoDeProdutosService;

    public MovimentacaoDeProdutoController(IMovimentacaoDeProdutosService movimentacaoDeProdutosService)
    {
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
    }

    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoMovimentacaoDeProdutoDto paginacaoMovimentacaoDeProdutoDto)
    {
        var paginacao = await _movimentacaoDeProdutosService.GetPaginacaoAsync(paginacaoMovimentacaoDeProdutoDto);
        return Ok(paginacao);
    }
}
