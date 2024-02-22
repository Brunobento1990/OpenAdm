using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class BannerRepository(ParceiroContext parceiroContext)
        : GenericRepository<Banner>(parceiroContext), IBannerRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<Banner?> GetBannerByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Banners
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Banner>> GetBannersAsync()
    {
        return await _parceiroContext
            .Banners
            .Where(x => x.Ativo)
            .ToListAsync();
    }

    public async Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(FilterModel<Banner> filterModel)
    {
        var (total, values) = await _parceiroContext
                .Banners
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Banner>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}
