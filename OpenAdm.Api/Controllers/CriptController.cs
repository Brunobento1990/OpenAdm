using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Application.Adapters;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("cript")]
public class CriptController : ControllerBase
{
    private readonly OpenAdmContext _openAdmContext;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly ParceiroContext _parceiroContext;
    public CriptController(OpenAdmContext openAdmContext, IParceiroAutenticado parceiroAutenticado, ParceiroContext parceiroContext)
    {
        _openAdmContext = openAdmContext;
        _parceiroAutenticado = parceiroAutenticado;
        _parceiroContext = parceiroContext;
    }

    [HttpPost("Criptar")]
    public async Task<IActionResult> Criptar(BodyCript bodyCript)
    {
        var id = Guid.Parse("95114744-982f-4e09-a38d-a1e2b702ca1f");
        var empresa = await _openAdmContext
            .ConfiguracoesParceiro
            .OrderByDescending(x => x.DataDeCriacao)
            .FirstOrDefaultAsync(x => x.ParceiroId == id);

        if(empresa != null)
        {
            empresa.DominioSiteAdm = "http://localhost:7154/";
            _openAdmContext.Update(empresa);
            await _openAdmContext.SaveChangesAsync();
        }


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
