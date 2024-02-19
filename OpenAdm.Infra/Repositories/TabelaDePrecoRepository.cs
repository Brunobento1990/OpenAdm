using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.Repositories;

public class TabelaDePrecoRepository : GenericRepository<TabelaDePreco>, ITabelaDePrecoRepository
{
    private readonly ParceiroContext _parceiroContext;
    public TabelaDePrecoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<TabelaDePreco?> GetTabelaDePrecoAtivaAsync()
    {
        return await _parceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .Include(x => x.ItensTabelaDePreco)
            .FirstOrDefaultAsync(x => x.AtivaEcommerce);
    }
}
