using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class BannerCached : IBannerRepository
{
    private readonly BannerRepository _bannerRepository;
    private readonly ICachedService<Banner> _cachedService;
    private static readonly string _keyList = "banners";
    public BannerCached(
        BannerRepository bannerRepository,
        ICachedService<Banner> cachedService)
    {
        _bannerRepository = bannerRepository;
        _cachedService = cachedService;
    }

    public async Task<Banner> AddAsync(Banner entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _bannerRepository.AddAsync(entity);
    }

    public async Task<bool> DeleteAsync(Banner entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _bannerRepository.DeleteAsync(entity);
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

    public Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(FilterModel<Banner> filterModel)
        => _bannerRepository.PaginacaoAsync(filterModel);

    public async Task<Banner> UpdateAsync(Banner entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _bannerRepository.UpdateAsync(entity);
    }

    public Task<PaginacaoViewModel<Banner>> PaginacaoAsync(FilterModel<Banner> filterModel)
        => _bannerRepository.PaginacaoAsync(filterModel);

    public Task<IList<Banner>> PaginacaoDropDownAsync(PaginacaoDropDown<Banner> paginacaoDropDown)
        => _bannerRepository.PaginacaoDropDownAsync(paginacaoDropDown);

    public Task<int> SaveChangesAsync()
     => _bannerRepository.SaveChangesAsync();

    public Task<Banner> AdicionarAsync(Banner entity)
        => _bannerRepository.AdicionarAsync(entity);

    public void Update(Banner entity)
        => _bannerRepository.Update(entity);
}
