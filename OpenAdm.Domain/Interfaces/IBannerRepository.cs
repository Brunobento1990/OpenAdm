using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IBannerRepository : IGenericBaseRepository<Banner>
{
    Task<IList<Banner>> GetBannersAsync(Guid parceiroId);
    Task<Banner?> GetBannerByIdAsync(Guid id);
}
