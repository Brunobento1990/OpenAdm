using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Application.Dtos.Banners;
using OpenAdm.Application.Models.Banners;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Model.PaginateDto;

namespace OpenAdm.Application.Services;

public class BannerService(IBannerRepository bannerRepository)
    : IBannerService
{
    private readonly IBannerRepository _bannerRepository = bannerRepository;

    public async Task<BannerViewModel> CreateBannerAsync(BannerCreateDto bannerCreateDto)
    {
        var banner = bannerCreateDto.ToEntity();

        await _bannerRepository.AddAsync(banner);

        return new BannerViewModel().ToModel(banner);
    }

    public async Task DeleteBannerAsync(Guid id)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var result = await _bannerRepository.DeleteAsync(banner);

        if (!result) throw new ExceptionApi();
    }

    public async Task<BannerViewModel> EditBannerAsync(BannerEditDto bannerEditDto)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(bannerEditDto.Id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        banner.Update(bannerEditDto.Foto, bannerEditDto.Ativo);

        await _bannerRepository.UpdateAsync(banner);

        return new BannerViewModel().ToModel(banner);
    }

    public async Task<BannerViewModel> GetBannerByIdAsync(Guid id)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        return new BannerViewModel().ToModel(banner);
    }

    public async Task<IList<BannerViewModel>> GetBannersAsync()
    {
        var banners = await _bannerRepository.GetBannersAsync();

        return banners
            .Select(banner => new BannerViewModel().ToModel(banner))
            .ToList();
    }

    public async Task<PaginacaoViewModel<BannerViewModel>> GetPaginacaoAsync(PaginacaoBannerDto paginacaoBannerDto)
    {
        var paginacao = await _bannerRepository.GetPaginacaoBannerAsync(paginacaoBannerDto);

        return new()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new BannerViewModel().ToModel(x)).ToList()
        };
    }
}
