using Domain.Pkg.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("cript")]
public class CriptController : ControllerBase
{
    [HttpPost]
    public IActionResult Criptar(BodyCript bodyCript)
    {
        var conn = CryptographyGeneric.Encrypt(bodyCript.Code);
        return Ok(conn);
    }
}

public class BodyCript
{
    public string Code { get; set; } = string.Empty;
}
