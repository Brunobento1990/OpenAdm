using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Tokens;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBaseApi
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CretaeUsuario(CreateUsuarioDto createUsuarioDto)
    {
        try
        {
            var responseCreateUsuario = await _usuarioService.CreateUsuarioAsync(createUsuarioDto);
            return Ok(responseCreateUsuario);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("get-conta")]
    public async Task<IActionResult> GetConta()
    {
        try
        {
            var usuarioViewModel = await _usuarioService.GetUsuarioByIdAsync();
            return Ok(usuarioViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpGet("list")]
    public async Task<IActionResult> GetUsuarios()
    {
        try
        {
            var usuariosViewModel = await _usuarioService.GetAllUsuariosAsync();
            return Ok(usuariosViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUsuario(UpdateUsuarioDto updateUsuarioDto)
    {
        try
        {
            var result = await _usuarioService.UpdateUsuarioAsync(updateUsuarioDto);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("update-senha")]
    public async Task<IActionResult> UpdateSenha(UpdateSenhaUsuarioDto updateSenhaUsuarioDto)
    {
        try
        {
            await _usuarioService.TrocarSenhaAsync(updateSenhaUsuarioDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
