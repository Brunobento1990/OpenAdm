using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("usuario")]
[AcessoParceiro]
public class EnderecoUsuarioController : ControllerBase
{
    private readonly IEnderecoUsuarioService _enderecoUsuarioService;

    public EnderecoUsuarioController(IEnderecoUsuarioService enderecoUsuarioService)
    {
        _enderecoUsuarioService = enderecoUsuarioService;
    }

    [HttpPost("endereco/criar-ou-atualizar")]
    [Autentica]
    [ProducesResponseType<EnderecoViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> CriarOuAtualizarEndereco(EnderecoDto enderecoDto)
    {
        var response = await _enderecoUsuarioService.CriarOuEditarAsync(enderecoDto);
        return Ok(response);
    }
}
