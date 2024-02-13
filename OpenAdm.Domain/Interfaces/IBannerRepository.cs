using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IBannerRepository : IGenericRepository<Banner>
{
    IQueryable<Banner> GetBannersAsync();
}
