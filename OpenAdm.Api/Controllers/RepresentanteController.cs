using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.Representantes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Representantes;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("representantes")]
[AcessoParceiro]
[Autentica]
[IsFuncionario]
public sealed class RepresentanteController(IRepresentanteService representanteService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<RepresentanteViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarAsync(CriarRepresentanteDto dto) =>
        (await representanteService.CriarAsync(dto)).ToActionResult();

    [HttpPut]
    [ProducesResponseType<RepresentanteViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditarAsync(EditarRepresentanteDto dto) =>
        (await representanteService.EditarAsync(dto)).ToActionResult();

    [HttpGet("{id:guid}")]
    [ProducesResponseType<RepresentanteViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VisualizarAsync(Guid id) =>
        (await representanteService.VisualizarAsync(id)).ToActionResult();

    [HttpPut("{id:guid}/ativo/{ativo:bool}")]
    [ProducesResponseType<ResultadoPadraoViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InativarAtivarAsync(Guid id, bool ativo) =>
        (await representanteService.InativarAtivarAsync(id, ativo)).ToActionResult();

    [HttpPost("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<RepresentanteViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PaginarAsync(PaginacaoRepresentanteDto filtro) =>
        (await representanteService.PaginarAsync(filtro)).ToActionResult();
}
