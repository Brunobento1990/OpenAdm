using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.MovimentosDeProdutos;
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

    [HttpPost("paginacao")]
    public async Task<IActionResult> Paginacao(PaginacaoMovimentacaoDeProdutoDto paginacaoMovimentacaoDeProdutoDto)
    {
        var paginacao = await _movimentacaoDeProdutosService.GetPaginacaoAsync(paginacaoMovimentacaoDeProdutoDto);
        return Ok(paginacao);
    }

    [HttpPost("relatorio")]
    public async Task<IActionResult> Relatorio(RelatorioMovimentoDeProdutoDto relatorioMovimentoDeProdutoDto)
    {
        var pdf = await _movimentacaoDeProdutosService.GerarRelatorioAsync(relatorioMovimentoDeProdutoDto);
        return Ok(new
        {
            pdf
        });
    }
}
