using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Factories.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class BannerRepository(IParceiroContextFactory parceiroContextFactory)
        : GenericRepository<Banner>(parceiroContextFactory), IBannerRepository
{
    private readonly IParceiroContextFactory _parceiroContextFactory = parceiroContextFactory;

    public async Task<IQueryable<Banner>> GetBannersAsync()
    {
        var context = await _parceiroContextFactory
            .CreateParceiroContextAsync();

        return context.Banners;
    }
}
