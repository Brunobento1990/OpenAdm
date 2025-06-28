using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("cep")]
[AcessoParceiro]
public class CepController : ControllerBase
{
    private readonly IConsultaCepService _consultaCepService;
    public CepController(IConsultaCepService consultaCepService)
    {
        _consultaCepService = consultaCepService;
    }

    [HttpGet("consultar")]
    [ProducesResponseType<EnderecoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [Autentica]
    public async Task<IActionResult> Consultar([FromQuery] string cep)
    {
        var result = await _consultaCepService.ConsultarAsync(cep);
        return Ok(result);
    }
}
