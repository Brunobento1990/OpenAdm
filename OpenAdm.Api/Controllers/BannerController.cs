using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Banners;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Banners;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("banners")]
[AcessoParceiro]
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

    [HttpGet("teste")]
    public IActionResult Teste()
    {
        return Ok(Criptografia.Encrypt("User ID=postgres; Password=1234; Host=localhost; Port=4814; Database=dev; Pooling=true;"));
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("paginacao")]
    [ProducesResponseType<PaginacaoViewModel<BannerViewModel>>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Paginacao(PaginacaoBannerDto paginacaoBannerDto)
    {
        var paginacaoViewModel = await _bannerService.GetPaginacaoAsync(paginacaoBannerDto);
        return Ok(paginacaoViewModel);
    }

    [Autentica]
    [IsFuncionario]
    [HttpPost("create")]
    [ProducesResponseType<BannerViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> Create(BannerCreateDto bannerCreateDto)
    {
        var result = await _bannerService.CreateBannerAsync(bannerCreateDto);
        return Ok(result);
    }

    [ResponseCache(CacheProfileName = "Default300")]
    [Autentica]
    [IsFuncionario]
    [HttpGet("get-banner")]
    [ProducesResponseType<BannerViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
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
    [ProducesResponseType<BannerViewModel>(200)]
    [ProducesResponseType<ErrorResponse>(400)]
    public async Task<IActionResult> EditBanner(BannerEditDto bannerEditDto)
    {
        var bannerViewModel = await _bannerService.EditBannerAsync(bannerEditDto);
        return Ok(bannerViewModel);
    }
}
