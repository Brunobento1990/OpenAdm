using OpenAdm.Application.Models;
using OpenAdm.Domain.Model.PaginateDto;
using OpenAdm.Domain.Model;
using OpenAdm.Application.Dtos.Banners;

namespace OpenAdm.Application.Interfaces;

public interface IBannerService
{
    IEnumerable<BannerViewModel> GetBannersAsync();
    Task<PaginacaoViewModel<BannerViewModel>> GetPaginacaoAsync(PaginacaoBannerDto paginacaoBannerDto);
    Task<BannerViewModel> CreateBannerAsync(BannerCreateDto bannerCreateDto);
    Task DeleteBannerAsync(Guid id);
    Task<BannerViewModel> GetBannerByIdAsync(Guid id);
    Task<BannerViewModel> EditBannerAsync(BannerEditDto bannerEditDto);
}
