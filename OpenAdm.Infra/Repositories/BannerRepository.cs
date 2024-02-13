using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.EntityConfiguration;
using OpenAdm.Infra.Factories.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class BannerRepository(ParceiroContext parceiroContext)
        : GenericRepository<Banner>(parceiroContext), IBannerRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public IQueryable<Banner> GetBannersAsync()
    {
        return _parceiroContext.Banners;
    }
}
