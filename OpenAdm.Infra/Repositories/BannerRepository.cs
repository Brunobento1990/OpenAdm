using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class BannerRepository(AppDbContext parceiroContext)
        : GenericBaseRepository<Banner>(parceiroContext), IBannerRepository
{
    public async Task<Banner?> GetBannerByIdAsync(Guid id)
    {
        return await AppDbContext
            .Banners
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Banner>> GetBannersAsync(Guid parceiroId)
    {
        return await AppDbContext
            .Banners
            .OrderByDescending(x => x.DataDeCriacao)
            .Take(5)
            .Where(x => x.Ativo && x.ParceiroId == parceiroId)
            .ToListAsync();
    }
}
