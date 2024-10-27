using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("fatura")]
[Autentica]
[IsFuncionario]
[AutenticaParceiro]
public class FaturaController : ControllerBase
{
    private readonly IFaturaService _faturaService;

    public FaturaController(IFaturaService faturaService)
    {
        _faturaService = faturaService;
    }

    [HttpPost("criar")]
    [ProducesResponseType<PaginacaoViewModel<FaturaViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Criar(FaturaCriarAdmDto faturaCriarAdmDto)
    {
        var result = await _faturaService.CriarAdmAsync(faturaCriarAdmDto);
        return Ok(result);
    }
}
