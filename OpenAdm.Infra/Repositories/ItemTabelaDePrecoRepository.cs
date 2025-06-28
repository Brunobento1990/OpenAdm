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
        await ParceiroContext.AddRangeAsync(itensTabelaDePrecos);
        await ParceiroContext.SaveChangesAsync();
    }

    public async Task DeleteItensTabelaDePrecoByProdutoIdAsync(Guid produtoId)
    {
        var itensTabelaDePreco = await ParceiroContext
            .ItensTabelaDePreco
            .Where(x => x.ProdutoId == produtoId)
            .ToListAsync();

        if (itensTabelaDePreco.Count > 0)
        {
            ParceiroContext.RemoveRange(itensTabelaDePreco);
        }
    }

    public async Task<ItemTabelaDePreco?> GetItemTabelaDePrecoByIdAsync(Guid id)
    {
        return await ParceiroContext
            .ItensTabelaDePreco
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<ItemTabelaDePreco>> GetItensTabelaDePrecoByIdAsync(Guid tabelaDePrecoId)
    {
        return await ParceiroContext
            .ItensTabelaDePreco
            .AsNoTracking()
            .Where(x => x.TabelaDePrecoId == tabelaDePrecoId)
            .ToListAsync();
    }

    public async Task<IList<ItemTabelaDePreco>> GetItensTabelaDePrecoByIdProdutosAsync(IList<Guid> produtosIds)
    {
        return await ParceiroContext
            .ItensTabelaDePreco
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();
    }

    public async Task<IList<ItemTabelaDePreco>> ObterPorPesoIdAsync(Guid pesoId)
    {
        return await ParceiroContext
            .ItensTabelaDePreco
            .Where(x => x.PesoId == pesoId)
            .ToListAsync();
    }

    public async Task<IList<ItemTabelaDePreco>> ObterPorTamanhoIdAsync(Guid tamanhoId)
    {
        return await ParceiroContext
            .ItensTabelaDePreco
            .Where(x => x.TamanhoId == tamanhoId)
            .ToListAsync();
    }
}
