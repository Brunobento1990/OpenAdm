using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IBannerRepository
{
    Task<IQueryable<Banner>> GetBannersAsync();
}
