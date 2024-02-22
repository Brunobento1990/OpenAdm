using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("home")]
public class HomeController : ControllerBaseApi
{
    private readonly IHomeSevice _homeEcommerSevice;

    public HomeController(IHomeSevice homeEcommerSevice)
    {
        _homeEcommerSevice = homeEcommerSevice;
    }

    [HttpGet("ecommerce")]
    [ResponseCache(CacheProfileName = "Defautl60")]
    public async Task<IActionResult> ListEcommerce()
    {
        try
        {
            var home = await _homeEcommerSevice.GetHomeEcommerceAsync();
            return Ok(home);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
