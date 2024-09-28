using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IBannerRepository : IGenericRepository<Banner>
{
    Task<IList<Banner>> GetBannersAsync();
    Task<Banner?> GetBannerByIdAsync(Guid id);
    Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(FilterModel<Banner> filterModel);
}
