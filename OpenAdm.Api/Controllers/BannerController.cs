using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Banners;
using OpenAdm.Application.Interfaces;

using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("banners")]
[AutenticaParceiro]
public class BannerController(IBannerService bannerService) : ControllerBase
{
    private readonly IBannerService _bannerService = bannerService;

    [HttpGet("list")]
    [ResponseCache(CacheProfileName = "Default300")]
    public async Task<IActionResult> ListarBanners()
    {
        var bannersViewModel = await _bannerService.GetBannersAsync();
        return Ok(bannersViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("paginacao")]
    public async Task<IActionResult> Paginacao(PaginacaoBannerDto paginacaoBannerDto)
    {
        var paginacaoViewModel = await _bannerService.GetPaginacaoAsync(paginacaoBannerDto);
        return Ok(paginacaoViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("create")]
    public async Task<IActionResult> Create(BannerCreateDto bannerCreateDto)
    {
        var result = await _bannerService.CreateBannerAsync(bannerCreateDto);
        return Ok(result);
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [Autentica]
    [IsFuncionario]
    [HttpGet("get-banner")]
    public async Task<IActionResult> GetBanner([FromQuery] Guid id)
    {
        var bannerViewModel = await _bannerService.GetBannerByIdAsync(id);
        return Ok(bannerViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteBanner([FromQuery] Guid id)
    {
        await _bannerService.DeleteBannerAsync(id);
        return Ok();
    }

    [Autentica]
    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> EditBanner(BannerEditDto bannerEditDto)
    {
        var bannerViewModel = await _bannerService.EditBannerAsync(bannerEditDto);
        return Ok(bannerViewModel);
    }
}
