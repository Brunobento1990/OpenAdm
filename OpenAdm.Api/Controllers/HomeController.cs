using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("home")]
[AcessoParceiro]
public class HomeController : ControllerBase
{
    private readonly IHomeSevice _homeEcommerSevice;

    public HomeController(IHomeSevice homeEcommerSevice)
    {
        _homeEcommerSevice = homeEcommerSevice;
    }

    [HttpGet("adm")]
    [ProducesResponseType<HomeAdmViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    [Autentica]
    [IsFuncionario]
    public async Task<IActionResult> ListAdm()
    {
        var home = await _homeEcommerSevice.GetHomeAdmAsync();
        return Ok(home);
    }
}
