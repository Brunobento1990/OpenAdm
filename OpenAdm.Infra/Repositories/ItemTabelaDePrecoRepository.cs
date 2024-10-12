using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ItemTabelaDePrecoRepository : GenericRepository<ItemTabelaDePreco>, IItemTabelaDePrecoRepository
{
    public ItemTabelaDePrecoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task AddRangeAsync(IList<ItemTabelaDePreco> itensTabelaDePrecos)
    {
        await _parceiroContext.AddRangeAsync(itensTabelaDePrecos);
        await _parceiroContext.SaveChangesAsync();
    }

    public async Task DeleteItensTabelaDePrecoByProdutoIdAsync(Guid produtoId)
    {
        var itensTabelaDePreco = await _parceiroContext
            .ItensTabelaDePreco
            .Where(x => x.ProdutoId == produtoId)
            .ToListAsync();

        _parceiroContext.RemoveRange(itensTabelaDePreco);

        await _parceiroContext.SaveChangesAsync();
    }

    public async Task<ItemTabelaDePreco?> GetItemTabelaDePrecoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .ItensTabelaDePreco
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<ItemTabelaDePreco>> GetItensTabelaDePrecoByIdProdutosAsync(IList<Guid> produtosIds)
    {
        return await _parceiroContext
            .ItensTabelaDePreco
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();
    }
}
