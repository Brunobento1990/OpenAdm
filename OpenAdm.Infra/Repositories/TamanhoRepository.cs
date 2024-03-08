using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class TamanhoRepository : GenericRepository<Tamanho>, ITamanhoRepository
{
    private readonly ParceiroContext _parceiroContext;
    public TamanhoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoTamanhosAsync(IList<Guid> ids)
    {
        return await _parceiroContext
            .Tamanhos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Descricao);
    }

    public async Task<PaginacaoViewModel<Tamanho>> GetPaginacaoTamanhoAsync(FilterModel<Tamanho> filterModel)
    {
        var (total, values) = await _parceiroContext
                .Tamanhos
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

    public async Task<Tamanho?> GetTamanhoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Tamanhos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Tamanho>> GetTamanhosAsync()
    {
        return await _parceiroContext
            .Tamanhos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IList<Tamanho>> GetTamanhosByIdsAsync(IList<Guid> ids)
    {
        return await _parceiroContext
            .Tamanhos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
}
