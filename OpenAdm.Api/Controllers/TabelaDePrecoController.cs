using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[IsFuncionario]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("tabelas-de-precos")]
public class TabelaDePrecoController : ControllerBaseApi
{
    private readonly ITabelaDePrecoService _tabelaDePrecoService;

    public TabelaDePrecoController(ITabelaDePrecoService tabelaDePrecoService)
    {
        _tabelaDePrecoService = tabelaDePrecoService;
    }

    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoTabelaDePrecoDto paginacaoTabelaDePrecoDto)
    {
        try
        {
            var paginacao = await _tabelaDePrecoService.GetPaginacaoTabelaViewModelAsync(paginacaoTabelaDePrecoDto);
            return Ok(paginacao);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
