using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("movimentacao-de-produto")]
[Authorize(AuthenticationSchemes = "Bearer")]
[IsFuncionario]
public class MovimentacaoDeProdutoController : ControllerBaseApi
{
    private readonly IMovimentacaoDeProdutosService _movimentacaoDeProdutosService;

    public MovimentacaoDeProdutoController(IMovimentacaoDeProdutosService movimentacaoDeProdutosService)
    {
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
    }

    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoMovimentacaoDeProdutoDto paginacaoMovimentacaoDeProdutoDto)
    {
        try
        {
            var paginacao = await _movimentacaoDeProdutosService.GetPaginacaoAsync(paginacaoMovimentacaoDeProdutoDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
