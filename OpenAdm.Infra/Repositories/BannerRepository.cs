using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class BannerRepository(ParceiroContext parceiroContext)
        : GenericRepository<Banner>(parceiroContext), IBannerRepository
{
    public async Task<Banner?> GetBannerByIdAsync(Guid id)
    {
        return await ParceiroContext
            .Banners
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Banner>> GetBannersAsync()
    {
        return await ParceiroContext
            .Banners
            .OrderBy(x => Guid.NewGuid())
            .Take(5)
            .Where(x => x.Ativo)
            .ToListAsync();
    }
}
