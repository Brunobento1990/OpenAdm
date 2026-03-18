using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracaoDeFreteModel;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("frete")]
public class FreteController : ControllerBase
{
    private readonly IFreteService _freteService;

    public FreteController(IFreteService freteService)
    {
        _freteService = freteService;
    }

    [HttpPost("cotar-frete")]
    [Autentica]
    [AcessoParceiro]
    [ProducesResponseType<CotacaoDeFreteViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CotarFrete(CotacaoFreteDTO cotacaoFreteDto)
    {
        var resultado = await _freteService.CotarFreteAsync(cotacaoFreteDto);
        return resultado.ToActionResult();
    }
}