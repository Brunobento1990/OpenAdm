using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Infra.Context;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("teste")]
public class TesteController : Controller
{
    //private readonly ParceiroContext _appDbContext;

    //public TesteController(ParceiroContext appDbContext)
    //{
    //    _appDbContext = appDbContext;
    //}

    //[HttpGet]
    //public async Task<IActionResult> Teste()
    //{
    //    var banners = await _appDbContext.Banners.ToListAsync();

    //    var query = $@"insert into ""Banners"" values {string.Join(",", banners.Select(x => $"('{x.Id}','{x.DataDeCriacao:yyyy-MM-dd HH:mm:ss}','{x.DataDeAtualizacao:yyyy-MM-dd HH:mm:ss}',{x.Numero}, '{x.Foto}', '{x.Ativo}', '{x.NomeFoto}')"))}";

    //    return Ok(query);
    //}
}
