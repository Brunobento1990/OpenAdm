using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ItemTabelaDePrecoRepository : GenericRepository<ItensTabelaDePreco>, IItemTabelaDePrecoRepository
{
    private readonly ParceiroContext _parceiroContext;
    public ItemTabelaDePrecoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<ItensTabelaDePreco?> GetItemTabelaDePrecoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .ItensTabelaDePreco
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
