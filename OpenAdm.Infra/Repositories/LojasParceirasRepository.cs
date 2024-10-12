using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class LojasParceirasRepository : GenericRepository<LojaParceira>, ILojasParceirasRepository
{
    public LojasParceirasRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IList<string?>> GetFotosLojasParceirasAsync()
    {
        return await _parceiroContext
            .LojasParceiras
            .Where(x => x.Foto != null)
            .OrderBy(x => Guid.NewGuid())
            .Take(5)
            .Select(x => x.Foto)
            .ToListAsync();
    }

    public async Task<LojaParceira?> GetLojaParceiraByIdAsync(Guid id)
    {
        return await _parceiroContext
            .LojasParceiras
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginacaoViewModel<LojaParceira>> GetPaginacaoLojasParceirasAsync(FilterModel<LojaParceira> filterModel)
    {
        var (total, values) = await _parceiroContext
                .LojasParceiras
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<LojaParceira>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}
