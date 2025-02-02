using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("cnpj")]
public class CnpjConsultaController : ControllerBase
{
    private readonly ICnpjConsultaService _cnpjConsultaService;

    public CnpjConsultaController(ICnpjConsultaService cnpjConsultaService)
    {
        _cnpjConsultaService = cnpjConsultaService;
    }

    [HttpGet("consulta")]
    [ProducesResponseType<ConsultaCnpjResponse>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Consulta([FromQuery] string cnpj)
    {
        var response = await _cnpjConsultaService.ConsultaCnpjAsync(cnpj);
        return Ok(response);
    }
}
