using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("banners")]
public class BannerController(IBannerService bannerService) : ControllerBaseApi
{
    private readonly IBannerService _bannerService = bannerService;

    [HttpGet("list")]
    public async Task<IActionResult> ListarBanners()
    {
        try
        {
            var bannersViewModel = _bannerService.GetBannersAsync();
            return Ok(bannersViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
