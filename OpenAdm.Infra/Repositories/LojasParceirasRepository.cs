using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class LojasParceirasRepository : GenericBaseRepository<LojaParceira>, ILojasParceirasRepository
{
    public LojasParceirasRepository(AppDbContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IList<string?>> GetFotosLojasParceirasAsync(Guid parceiroId)
    {
        return await AppDbContext
            .LojasParceiras
            .Where(x => x.Foto != null)
            .OrderByDescending(x => x.DataDeCriacao)
            .Take(5)
            .Where(x => x.ParceiroId == parceiroId)
            .Select(x => x.Foto)
            .ToListAsync();
    }

    public async Task<LojaParceira?> GetLojaParceiraByIdAsync(Guid id)
    {
        return await AppDbContext
            .LojasParceiras
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<LojaParceira>> GetLojasParceirasAsync(Guid parceiroId)
    {
        return await AppDbContext
            .LojasParceiras
            .AsNoTracking()
            .Where(x => x.ParceiroId == parceiroId)
            .ToListAsync();
    }
}
