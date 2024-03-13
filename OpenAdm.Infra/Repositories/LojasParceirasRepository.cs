using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class LojasParceirasRepository : GenericRepository<LojasParceiras>, ILojasParceirasRepository
{
    private readonly ParceiroContext _parceiroContext;
    public LojasParceirasRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<IList<string?>> GetFotosLojasParceirasAsync()
    {
        return await _parceiroContext
            .LojasParceiras
            .Where(x => x.Foto != null)
            .Select(x => x.Foto)
            .ToListAsync();
    }

    public async Task<LojasParceiras?> GetLojaParceiraByIdAsync(Guid id)
    {
        return await _parceiroContext
            .LojasParceiras
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginacaoViewModel<LojasParceiras>> GetPaginacaoLojasParceirasAsync(FilterModel<LojasParceiras> filterModel)
    {
        var (total, values) = await _parceiroContext
                .LojasParceiras
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<LojasParceiras>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}
