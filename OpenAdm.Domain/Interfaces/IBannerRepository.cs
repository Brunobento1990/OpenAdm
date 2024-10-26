using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IBannerRepository : IGenericRepository<Banner>
{
    Task<IList<Banner>> GetBannersAsync();
    Task<Banner?> GetBannerByIdAsync(Guid id);
}
