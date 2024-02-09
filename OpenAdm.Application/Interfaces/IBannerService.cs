using OpenAdm.Application.Models;

namespace OpenAdm.Application.Interfaces;

public interface IBannerService
{
    Task<IEnumerable<BannerViewModel>> GetBannersAsync();
}
