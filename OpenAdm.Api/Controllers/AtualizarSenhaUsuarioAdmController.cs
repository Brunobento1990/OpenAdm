using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("usuario")]
[Autentica]
[IsFuncionario]
public class AtualizarSenhaUsuarioAdmController : ControllerBase
{
    private readonly IAtualizarSenhaUsuarioAdmService _atualizarSenhaUsuarioAdmService;

    public AtualizarSenhaUsuarioAdmController(IAtualizarSenhaUsuarioAdmService atualizarSenhaUsuarioAdmService)
    {
        _atualizarSenhaUsuarioAdmService = atualizarSenhaUsuarioAdmService;
    }

    [HttpPost("atualizar-senha-adm")]
    public async Task<IActionResult> AtualizarSenhaAdm(AtualizarSenhaUsuarioAdmDto atualizarSenhaUsuarioAdmDto)
    {
        var result = await _atualizarSenhaUsuarioAdmService.AtualizarAsync(atualizarSenhaUsuarioAdmDto);

        return Ok(new
        {
            result
        });
    }
}
