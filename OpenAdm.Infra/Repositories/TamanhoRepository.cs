using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class TamanhoRepository : GenericRepository<Tamanho>, ITamanhoRepository
{
    public TamanhoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoTamanhosAsync(IList<Guid> ids)
    {
        return await _parceiroContext
            .Tamanhos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Descricao);
    }

    public async Task<IDictionary<Guid, Tamanho>> GetDictionaryTamanhosAsync(IList<Guid> ids)
    {
        return await _parceiroContext
            .Tamanhos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x);
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
            .OrderByDescending(x => x.Numero)
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
