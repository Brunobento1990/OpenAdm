using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Application.Models.Representantes;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers.Representante;

[ApiController]
[Route("representante/clientes")]
[AcessoParceiro]
[Autentica]
[IsRepresentante]
public sealed class ClienteController(
    IUsuarioService usuarioService,
    IHistoricoClienteRepresentanteService historicoClienteService,
    IUsuarioAutenticado usuarioAutenticado) : ControllerBase
{
    [HttpGet("historico")]
    [ProducesResponseType<HistoricoClienteRepresentanteViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterHistoricoAsync([FromQuery] Guid clienteId) =>
        Ok(await historicoClienteService.ObterAsync(clienteId, usuarioAutenticado.Id));

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
