using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Repositories;

public class TamanhoRepository : GenericRepository<Tamanho>, ITamanhoRepository
{
    public TamanhoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IList<DropDownItemModel>> BuscarDropDownAsync(DropDownFiltro filtro)
    {
        var search = filtro.Search?.Trim();
        Expression<Func<Tamanho, bool>> where = string.IsNullOrWhiteSpace(search)
            ? x => x.Ativo
            : x => x.Ativo && EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{search}%");

        return await BuscarDropDownAsync(
            filtro,
            where,
            x => x.Descricao,
            x => x.Id,
            x => new DropDownItemModel { Id = x.Id, Descricao = x.Descricao });
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoTamanhosAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Tamanhos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Descricao);
    }

    public async Task<IDictionary<Guid, Tamanho>> GetDictionaryTamanhosAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Tamanhos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x);
    }

    public async Task<Tamanho?> GetTamanhoByIdAsync(Guid id)
    {
        return await ParceiroContext
            .Tamanhos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Tamanho>> GetTamanhosAsync()
    {
        return await ParceiroContext
            .Tamanhos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .ToListAsync();
    }

    public async Task<IList<Tamanho>> GetTamanhosByIdsAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Tamanhos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
}
