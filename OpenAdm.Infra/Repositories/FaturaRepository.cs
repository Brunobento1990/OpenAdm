using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class FaturaRepository : GenericRepository<Fatura>, IFaturaRepository
{
    public FaturaRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<Fatura?> GetByIdAsync(Guid id)
    {
        return await ParceiroContext
            .Faturas
            .AsNoTracking()
            .Include(x => x.Parcelas)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Fatura?> GetByIdCompletaAsync(Guid id)
    {
        return await ParceiroContext
            .Faturas
            .AsNoTracking()
            .Include(x => x.Parcelas)
            .Include(x => x.Usuario)
            .Include(x => x.Pedido)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
