using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Fretes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("frete")]
[Autentica]
public class FreteController : ControllerBase
{
    private readonly IFreteService _freteService;

    public FreteController(IFreteService freteService)
    {
        _freteService = freteService;
    }

    [HttpPost]
    [ProducesResponseType<ErrorResponse>(400)]
    [ProducesResponseType<ErrorResponse>(401)]
    [ProducesResponseType<FreteViewModel>(200)]
    public async Task<IActionResult> Calcular(CalcularFretePedidoDto calcularFreteDto)
    {
        var response = await _freteService.CalcularAsync(calcularFreteDto);
        return Ok(response);
    }
}
