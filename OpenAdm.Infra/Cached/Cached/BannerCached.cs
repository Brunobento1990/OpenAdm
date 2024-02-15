using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.PaginateDto;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class BannerCached : GenericRepository<Banner>, IBannerRepository
{
    private readonly BannerRepository _bannerRepository;
    private readonly ICachedService<Banner> _cachedService;
    public BannerCached(
        ParceiroContext parceiroContext,
        BannerRepository bannerRepository,
        ICachedService<Banner> cachedService)
            : base(parceiroContext)
    {
        _bannerRepository = bannerRepository;
        _cachedService = cachedService;
    }

    public async Task<Banner?> GetBannerByIdAsync(Guid id)
    {
        var banner = await _cachedService.GetItemAsync(id.ToString());

        if (banner == null)
        {
            banner = await _bannerRepository.GetBannerByIdAsync(id);
            if (banner != null)
            {
                await _cachedService.SetItemAsync(id.ToString(), banner);
            }
        }

        return banner;
    }

    public async Task<IList<Banner>> GetBannersAsync()
    {
        var banners = await _cachedService.GetListItemAsync("banners");

        if (banners == null)
        {
            banners = await _bannerRepository.GetBannersAsync();

            if (banners.Count > 0)
            {
                await _cachedService.SetListItemAsync("banners", banners);
            }
        }

        return banners;
    }

    public async Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(PaginacaoBannerDto paginacaoBannerDto)
    {
        return await _bannerRepository.GetPaginacaoBannerAsync(paginacaoBannerDto);
    }
}
