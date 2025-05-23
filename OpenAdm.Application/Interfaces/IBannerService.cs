﻿using OpenAdm.Domain.Model;
using OpenAdm.Application.Dtos.Banners;
using OpenAdm.Application.Models.Banners;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Interfaces;

public interface IBannerService
{
    Task<IList<BannerViewModel>> GetBannersAsync();
    Task<PaginacaoViewModel<BannerViewModel>> GetPaginacaoAsync(FilterModel<Banner> paginacaoBannerDto);
    Task<BannerViewModel> CreateBannerAsync(BannerCreateDto bannerCreateDto);
    Task DeleteBannerAsync(Guid id);
    Task<BannerViewModel> GetBannerByIdAsync(Guid id);
    Task<BannerViewModel> EditBannerAsync(BannerEditDto bannerEditDto);
}
