using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Infra.Context;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("teste")]
public class TesteController : Controller
{
    private readonly IHttpClientFrete _httpClientFrete;

    public TesteController(IHttpClientFrete httpClientFrete)
    {
        _httpClientFrete = httpClientFrete;
    }

    [HttpGet]
    public async Task<IActionResult> Teste()
    {
        var body = new CotacaoFreteRequest()
        {
            To = new()
            {
                Postal_code = "89270614"
            },
            From = new()
            {
                Postal_code = "88316301"
            },
            Products =
            [
                new()
                {
                    Height = 1,
                    Id = Guid.NewGuid().ToString(),
                    Length = 1,
                    Quantity = 32,
                    Weight = 1,
                    Width = 1
                }
            ]
        };
        var response = await _httpClientFrete.CotarFreteAsync(body,
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI5NTYiLCJqdGkiOiI5YzQ3MmU5NGRmMjYyMzUwYWFiNzE5YjhhZmI2MTdlN2ZkYmRmMjk2YzJkOTJlMzJlNGEyZjg4YWJlZjAwYjYwOWEwMDc5MTk4ZDAyZjBlZSIsImlhdCI6MTc3MzM1MDUzOC4xMTA0LCJuYmYiOjE3NzMzNTA1MzguMTEwNDAzLCJleHAiOjE4MDQ4ODY1MzguMDk2NDQ4LCJzdWIiOiI5ZDQ3MWE1Yy00MTMwLTRlYzYtOWNmYi0yNzcwNmE4N2QzY2EiLCJzY29wZXMiOlsic2hpcHBpbmctY2FsY3VsYXRlIl19.lu2imfA8Q0_b_9QFBZcMcY0z1X1Kqe5nOEU_sxeDh2VubHo5tZ801jo1TSyN_bFPbDDMCyLcNC3mLRJXBDrkrp5PzJ87HItP6jc-D2raMl1C-0ENyZHTCFUpS9mplZyTZIWxyYOX-Y-zLNjjQlkoDbe6hOBSz9kG2B6G9viyGVrW1sUSnZj7DZlyE5Felecu6NS_NjTSSiG2_B1HzTKIXmg3395CYogHLyBwiIFqzDkCQufvNmrJov352aEL3-HNQy2eON6JhYjtO8ZGwO4Lt7Zwts4WBAk5ucHxAf-68K0WN2oQyB-SELDfEROh_ywZ4tnyhWD93rtytyPPT4Zbe2f3WNEtcKcN1aTk6SphdTBLi_aF4fMp_RkUQ20A2CT9E4TNJwmq0SANHxA028pE8ze47PL8Q8kqZPlTYoJtMd-uY728_9b048VthcTBUOf6Y0SNFEykoi3F5uQ4pxNrICi0aYVsA0SbpIlHGG3_xPGbWmEnHrtzSwlG81sKLvf7ehYsp4bAm4tjs_dk70SfcPvfFPMGOOb75rxuDAjEZ12uXQLwkkgyiUEpGIA1IwblR8K5iQWEzS81GmudMpmSbrkzWif8wWBNEJiuEHcMVpEAmyL_Pz3qebShI5EekC7jx3B1uMqOz0RdzlxtUhr0-i7lJ3Sr62j7REyeI1kOuXw");
        return response.ToActionResult();
    }
}