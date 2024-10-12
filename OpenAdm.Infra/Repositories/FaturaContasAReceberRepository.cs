using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class FaturaContasAReceberRepository : GenericRepository<FaturaContasAReceber>, IFaturaContasAReceberRepository
{
    public FaturaContasAReceberRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<FaturaContasAReceber?> GetByIdAsync(Guid id)
    {
        return await _parceiroContext
            .FaturasContasAReceber
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<FaturaContasAReceber>> GetByPedidoIdAsync(Guid pedidoId, StatusFaturaContasAReceberEnum? statusFaturaContasAReceberEnum)
    {
        var query = _parceiroContext
            .FaturasContasAReceber
            .AsNoTracking()
            .Include(x => x.ContasAReceber)
            .Where(x => x.ContasAReceber.PedidoId == pedidoId);

        if (statusFaturaContasAReceberEnum.HasValue)
        {
            query = query.Where(x => x.Status == statusFaturaContasAReceberEnum);
        }

        return await query
            .ToListAsync();
    }
}
