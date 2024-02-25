using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Banners;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Mensageria.Interfaces;
using OpenAdm.Application.Models.Banners;
using OpenAdm.Infra.Paginacao;

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
            var bannersViewModel = await _bannerService.GetBannersAsync();
            return Ok(bannersViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoBannerDto paginacaoBannerDto)
    {
        try
        {
            var paginacaoViewModel = await _bannerService.GetPaginacaoAsync(paginacaoBannerDto);
            return Ok(paginacaoViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpPost("create")]
    public async Task<IActionResult> Create(BannerCreateDto bannerCreateDto)
    {
        try
        {
            var result = await _bannerService.CreateBannerAsync(bannerCreateDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpGet("get-banner")]
    public async Task<IActionResult> GetBanner([FromQuery] Guid id)
    {
        try
        {
            var bannerViewModel = await _bannerService.GetBannerByIdAsync(id);
            return Ok(bannerViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteBanner([FromQuery] Guid id)
    {
        try
        {
            await _bannerService.DeleteBannerAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [HttpPut("update")]
    public async Task<IActionResult> EditBanner(BannerEditDto bannerEditDto)
    {
        try
        {
            var bannerViewModel = await _bannerService.EditBannerAsync(bannerEditDto);
            return Ok(bannerViewModel);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
