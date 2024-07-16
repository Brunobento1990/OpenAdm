﻿using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [Autentica]
    [IsFuncionario]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoUsuarioDto paginacaoUsuarioDto)
    {
        var response = await _usuarioService.PaginacaoAsync(paginacaoUsuarioDto);
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CretaeUsuario(CreateUsuarioDto createUsuarioDto)
    {
        var responseCreateUsuario = await _usuarioService.CreateUsuarioAsync(createUsuarioDto);
        return Ok(responseCreateUsuario);
    }

    [Autentica]
    [HttpGet("get-conta")]
    public async Task<IActionResult> GetConta()
    {
        var usuarioViewModel = await _usuarioService.GetUsuarioByIdAsync();
        return Ok(usuarioViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpGet("list")]
    public async Task<IActionResult> GetUsuarios()
    {
        var usuariosViewModel = await _usuarioService.GetAllUsuariosAsync();
        return Ok(usuariosViewModel);
    }

    [Autentica]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUsuario(UpdateUsuarioDto updateUsuarioDto)
    {
        var result = await _usuarioService.UpdateUsuarioAsync(updateUsuarioDto);
        return Ok(result);
    }

    [Autentica]
    [HttpPut("update-senha")]
    public async Task<IActionResult> UpdateSenha(UpdateSenhaUsuarioDto updateSenhaUsuarioDto)
    {
        await _usuarioService.TrocarSenhaAsync(updateSenhaUsuarioDto);
        return Ok();
    }
}
