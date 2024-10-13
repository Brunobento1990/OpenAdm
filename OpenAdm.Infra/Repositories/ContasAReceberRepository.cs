using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ContasAReceberRepository : GenericRepository<ContasAReceber>, IContasAReceberRepository
{
    public ContasAReceberRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<ContasAReceber?> GetByIdAsync(Guid id)
    {
        return await _parceiroContext
            .ContasAReceber
            .AsNoTracking()
            .Include(x => x.Faturas)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
