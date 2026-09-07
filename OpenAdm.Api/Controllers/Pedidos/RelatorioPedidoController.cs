using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Autentica]
[AcessoParceiro]
public class RelatorioPedidoController : ControllerBase
{
    private readonly IRelatorioPedidoPorPeriodo _relatorioPedidoPorPeriodo;

    public RelatorioPedidoController(IRelatorioPedidoPorPeriodo relatorioPedidoPorPeriodo)
    {
        _relatorioPedidoPorPeriodo = relatorioPedidoPorPeriodo;
    }

    [HttpPost("relatorio-por-periodo")]
    public async Task<IActionResult> RelatorioPorPeriodo(RelatorioPedidoDto relatorioPedidoDto)
    {
        var relatorio = await _relatorioPedidoPorPeriodo.GetListagemAsync(relatorioPedidoDto);
        return Ok(relatorio);
    }

    [HttpPost("relatorio-por-periodo/imprimir")]
    [Produces("application/pdf")]
    public async Task<IActionResult> ImprimirRelatorioPorPeriodo(RelatorioPedidoDto relatorioPedidoDto)
    {
        var (pdf, _) = await _relatorioPedidoPorPeriodo.GetRelatorioAsync(relatorioPedidoDto);
        return File(pdf, "application/pdf", "relatorio-pedidos.pdf");
    }
}
