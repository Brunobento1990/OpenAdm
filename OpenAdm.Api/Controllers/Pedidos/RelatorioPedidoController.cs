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
        var values = await _relatorioPedidoPorPeriodo.GetRelatorioAsync(relatorioPedidoDto);
        return Ok(new { values.pdf, values.count });
    }
}
