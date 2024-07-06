using Domain.Pkg.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("cript")]
public class CriptController : ControllerBase
{
    [HttpPost("Criptar")]
    public IActionResult Criptar(BodyCript bodyCript)
    {
        var conn = CryptographyGeneric.Encrypt(bodyCript.Code);
        return Ok(conn);
    }

    [HttpPost("DeCriptar")]
    public IActionResult DeCriptar(BodyCript bodyCript)
    {
        var conn = CryptographyGeneric.Decrypt(bodyCript.Code);
        return Ok(conn);
    }
}

public class BodyCript
{
    public string Code { get; set; } = string.Empty;
}
