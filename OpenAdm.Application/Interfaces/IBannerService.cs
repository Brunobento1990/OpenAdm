using OpenAdm.Application.Models;

namespace OpenAdm.Application.Interfaces;

public interface IBannerService
{
    IEnumerable<BannerViewModel> GetBannersAsync();
}
