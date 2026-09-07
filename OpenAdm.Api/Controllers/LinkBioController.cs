using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.LinksBio;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.LinksBio;
using OpenAdm.Api.Extensions;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("link-bio")]
[AcessoParceiro]
[Autentica]
[IsFuncionario]
public sealed class LinkBioController(ILinkBioService service) : ControllerBase
{
    [HttpGet("configuracao")]
    [ProducesResponseType<LinkBioConfiguracaoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ObterConfiguracao() =>
        (await service.ObterConfiguracaoAsync()).ToActionResult();

    [HttpPost("configuracao/create-or-update")]
    [ProducesResponseType<LinkBioConfiguracaoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CriarOuEditarConfiguracao(LinkBioConfiguracaoDto dto) =>
        (await service.CriarOuEditarConfiguracaoAsync(dto)).ToActionResult();

    [HttpPost("links")]
    [ProducesResponseType<LinkBioItemViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CriarLink(LinkBioItemCreateDto dto) =>
        (await service.CriarLinkAsync(dto)).ToActionResult();

    [HttpPut("links")]
    [ProducesResponseType<LinkBioItemViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> EditarLink(LinkBioItemUpdateDto dto) =>
        (await service.EditarLinkAsync(dto)).ToActionResult();

    [HttpDelete("links/{id:guid}")]
    public async Task<IActionResult> ExcluirLink(Guid id)
    {
        return (await service.ExcluirLinkAsync(id)).ToActionResult();
    }

    [HttpPut("links/{id:guid}/status/{ativo:bool}")]
    public async Task<IActionResult> AlterarStatus(Guid id, bool ativo)
    {
        return (await service.AlterarStatusAsync(id, ativo)).ToActionResult();
    }

    [HttpPut("links/ordem")]
    public async Task<IActionResult> AlterarOrdem(LinkBioAlterarOrdemDto dto)
    {
        return (await service.AlterarOrdemAsync(dto)).ToActionResult();
    }

    [HttpPost("eventos")]
    [ProducesResponseType<LinkBioIndicadoresViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> ConsultarEventos(LinkBioEventosFiltroDto filtro) =>
        (await service.ConsultarEventosAsync(filtro)).ToActionResult();
}
