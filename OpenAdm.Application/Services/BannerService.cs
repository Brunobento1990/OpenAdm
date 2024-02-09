using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class BannerService(IBannerRepository bannerRepository)
    : IBannerService
{
    private readonly IBannerRepository _bannerRepository = bannerRepository;

    public async Task<IEnumerable<BannerViewModel>> GetBannersAsync()
    {
        var banners = await _bannerRepository.GetBannersAsync();

        return banners.Select(banner => new BannerViewModel().ToEntity(banner));
    }
}
