﻿using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.PaginateDto;

namespace OpenAdm.Domain.Interfaces;

public interface IBannerRepository : IGenericRepository<Banner>
{
    Task<IList<Banner>> GetBannersAsync();
    Task<Banner?> GetBannerByIdAsync(Guid id);
    Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(PaginacaoBannerDto paginacaoBannerDto);
}
