using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class PesoRepository : GenericRepository<Peso>, IPesoRepository
{
    private readonly ParceiroContext _parceiroContext;
    public PesoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoPesosAsync(IList<Guid> ids)
    {
        return await _parceiroContext
            .Pesos
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Id, x => x.Descricao);
    }

    public async Task<PaginacaoViewModel<Peso>> GetPaginacaoPesoAsync(FilterModel<Peso> filterModel)
    {
        var (total, values) = await _parceiroContext
                .Pesos
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Categoria>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }

    public async Task<Peso?> GetPesoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Pesos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Peso>> GetPesosAsync()
    {
        return await _parceiroContext
            .Pesos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .ToListAsync();
    }

    public async Task<IList<Peso>> GetPesosByIdsAsync(IList<Guid> ids)
    {
        return await _parceiroContext
            .Pesos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
}
