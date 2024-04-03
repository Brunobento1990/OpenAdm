using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class RelatorioPedidoController : ControllerBaseApi
{
    private readonly IRelatorioPedidoPorPeriodo _relatorioPedidoPorPeriodo;

    public RelatorioPedidoController(IRelatorioPedidoPorPeriodo relatorioPedidoPorPeriodo)
    {
        _relatorioPedidoPorPeriodo = relatorioPedidoPorPeriodo;
    }

    [HttpPost("relatorio-por-periodo")]
    public async Task<IActionResult> RelatorioPorPeriodo(RelatorioPedidoDto relatorioPedidoDto)
    {
        try
        {
            var values = await _relatorioPedidoPorPeriodo.GetRelatorioAsync(relatorioPedidoDto);
            return Ok(new { values.pdf, values.count });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
