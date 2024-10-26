using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Ceps;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("cep")]
[AutenticaParceiro]
public class CepController : ControllerBase
{
    private readonly IFreteService _freteService;

    public CepController(IFreteService freteService)
    {
        _freteService = freteService;
    }

    [HttpPost("cotar-frete")]
    [ProducesResponseType<FreteViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [Autentica]
    public async Task<IActionResult> CotarFrete(CotarFreteDto cotarFreteDto)
    {
        var result = await _freteService.CotarFreteAsync(cotarFreteDto);
        return Ok(result);
    }
}
