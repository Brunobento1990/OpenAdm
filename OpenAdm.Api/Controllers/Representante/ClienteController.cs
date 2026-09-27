using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers.Representante;

[ApiController]
[Route("representante/clientes")]
[AcessoParceiro]
[Autentica]
[IsRepresentante]
public sealed class ClienteController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpPost("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<UsuarioViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PaginarAsync(PaginacaoUsuarioDto filtro) =>
        Ok(await usuarioService.PaginacaoAsync(filtro));

    [HttpGet("{usuarioId:guid}")]
    [ProducesResponseType<UsuarioViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterPorIdAsync(Guid usuarioId) =>
        Ok(await usuarioService.GetUsuarioByIdAdmAsync(usuarioId));

    [HttpPost]
    [ProducesResponseType<UsuarioViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CriarAsync(CreateUsuarioAdminDto dto)
    {
        var response = await usuarioService.CreateUsuarioNoAdminAsync(dto);
        return Ok(response.Usuario);
    }

    [HttpPut("{usuarioId:guid}")]
    [ProducesResponseType<UsuarioViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> EditarAsync(Guid usuarioId, UpdateUsuarioDto dto) =>
        Ok(await usuarioService.UpdateUsuarioAsync(usuarioId, dto));
}
