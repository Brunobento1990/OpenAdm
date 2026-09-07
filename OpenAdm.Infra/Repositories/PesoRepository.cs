using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Repositories;

public class PesoRepository : GenericRepository<Peso>, IPesoRepository
{
    public PesoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IList<DropDownItemModel>> BuscarDropDownAsync(DropDownFiltro filtro)
    {
        var search = filtro.Search?.Trim();
        Expression<Func<Peso, bool>> where = string.IsNullOrWhiteSpace(search)
            ? x => x.Ativo
            : x => x.Ativo && EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{search}%");

        return await BuscarDropDownAsync(
            filtro,
            where,
            x => x.Descricao,
            x => x.Id,
            x => new DropDownItemModel { Id = x.Id, Descricao = x.Descricao });
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoPesosAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Descricao);
    }

    public async Task<IDictionary<Guid, Peso>> GetDictionaryPesosByIdsAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x);
    }
    
    public async Task<Peso?> GetPesoByIdAsync(Guid id)
    {
        return await ParceiroContext
            .Pesos
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Peso?> GetPesoByIdAsNoTrackingAsync(Guid id)
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Peso>> GetPesosAsync()
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderByDescending(x => x.Numero)
            .ToListAsync();
    }

    public async Task<IList<Peso>> GetPesosByIdsAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Pesos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
}
