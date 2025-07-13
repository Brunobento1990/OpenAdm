using Microsoft.AspNetCore.Mvc;
using OpenAdm.Infra.Context;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("teste")]
public class TesteController : Controller
{
    private readonly ParceiroContext _appDbContext;

    public TesteController(ParceiroContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    //[HttpGet]
    //public async Task<IActionResult> Teste()
    //{
    //    var banners = await _appDbContext.TopUsuarios.ToListAsync();

    //    var query = $@"insert into ""TopUsuarios"" 
    //        (""Id"", ""DataDeCriacao"", ""DataDeAtualizacao"", ""Numero"", ""TotalCompra"", ""TotalPedidos"", ""UsuarioId"", ""Usuario"",""ParceiroId"")
    //        values {string.Join(",\n", banners.Select(x => $"('{x.Id}','{x.DataDeCriacao:yyyy-MM-dd HH:mm:ss}','{x.DataDeAtualizacao:yyyy-MM-dd HH:mm:ss}',{x.Numero}, {x.TotalCompra}, {x.TotalPedidos}, '{x.UsuarioId}', '{x.Usuario}','c29f2d59-10a3-4b2e-bc97-5ef8047d7cec')"))}";

    //    return Ok(query);
    //}
}
