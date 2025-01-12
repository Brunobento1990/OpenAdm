using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class LojasParceirasRepository : GenericRepository<LojaParceira>, ILojasParceirasRepository
{
    public LojasParceirasRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IList<string?>> GetFotosLojasParceirasAsync()
    {
        return await ParceiroContext
            .LojasParceiras
            .Where(x => x.Foto != null)
            .OrderBy(x => Guid.NewGuid())
            .Take(5)
            .Select(x => x.Foto)
            .ToListAsync();
    }

    public async Task<LojaParceira?> GetLojaParceiraByIdAsync(Guid id)
    {
        return await ParceiroContext
            .LojasParceiras
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<LojaParceira>> GetLojasParceirasAsync()
    {
        return await ParceiroContext
            .LojasParceiras
            .AsNoTracking()
            .ToListAsync();
    }
}
