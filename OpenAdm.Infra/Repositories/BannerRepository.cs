using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.PaginateDto;
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

    public IQueryable<Banner> GetBannersAsync()
    {
        return _parceiroContext.Banners;
    }

    public async Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(PaginacaoBannerDto paginacaoBannerDto)
    {
        var (total, values) = await _parceiroContext
                .Banners
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Pedido>(x, paginacaoBannerDto.OrderBy))
                .WhereIsNotNull(paginacaoBannerDto.GetWhereBySearch())
                .CustomFilterAsync(paginacaoBannerDto);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}
