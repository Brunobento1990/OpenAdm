using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.LinksBio;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("link-bio/publico")]
[AcessoParceiro]
public sealed class LinkBioPublicoController(ILinkBioService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<LinkBioPaginaPublicaViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ObterPagina() =>
        (await service.ObterPaginaPublicaAsync()).ToActionResult();

    [HttpPost("links/{linkId:guid}/clique")]
    [ProducesResponseType(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> RegistrarClique(Guid linkId)
    {
        return (await service.RegistrarCliqueAsync(linkId)).ToActionResult();
    }
}
