using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.TransacoesFinanceiras;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("transacao-financeira")]
[AutenticaParceiro]
public class TransacaoFinanceiraController : ControllerBase
{
    private readonly ITransacaoFinanceiraService _transacaoFinanceiraService;

    public TransacaoFinanceiraController(ITransacaoFinanceiraService transacaoFinanceiraService)
    {
        _transacaoFinanceiraService = transacaoFinanceiraService;
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("periodo")]
    public async Task<IActionResult> Periodo(TransacaoFinanceiraNoPeriodoDto transacaoFinanceiraNoPeriodoDto)
    {
        var transacoes = await _transacaoFinanceiraService.TransacoesNoPeriodoAsync(transacaoFinanceiraNoPeriodoDto);
        return Ok(transacoes);
    }
}
