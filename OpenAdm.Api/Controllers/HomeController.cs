using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    private readonly IHomeSevice _homeEcommerSevice;

    public HomeController(IHomeSevice homeEcommerSevice)
    {
        _homeEcommerSevice = homeEcommerSevice;
    }

    //[ResponseCache(CacheProfileName = "Default300")]
    [HttpGet("adm")]
    public async Task<IActionResult> ListAdm()
    {
        var home = await _homeEcommerSevice.GetHomeAdmAsync();
        return Ok(home);
    }
}
