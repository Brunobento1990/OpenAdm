using Microsoft.AspNetCore.Mvc;
using OpenAdm.Domain.Helpers;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("cript")]
public class CriptController : ControllerBase
{
    [HttpPost("Criptar")]
    public IActionResult Criptar(BodyCript bodyCript)
    {
        if (!VariaveisDeAmbiente.IsDevelopment()) 
        {
            return Unauthorized();
        }
        var conn = Criptografia.Encrypt(bodyCript.Code);
        return Ok(conn);
    }

    [HttpPost("DeCriptar")]
    public IActionResult DeCriptar(BodyCript bodyCript)
    {
        if (!VariaveisDeAmbiente.IsDevelopment())
        {
            return Unauthorized();
        }
        var conn = Criptografia.Decrypt(bodyCript.Code);
        return Ok(conn);
    }
}

public class BodyCript
{
    public string Code { get; set; } = string.Empty;
}
