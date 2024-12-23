using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.Repositories;

public class TabelaDePrecoRepository : GenericRepository<TabelaDePreco>, ITabelaDePrecoRepository
{
    public TabelaDePrecoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<IList<TabelaDePreco>> GetAllTabelaDePrecoAsync()
    {
        return await ParceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetCountTabelaDePrecoAsync()
    {
        return await ParceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .CountAsync();
    }

    public async Task<TabelaDePreco?> GetTabelaDePrecoAtivaAsync()
    {
        return await ParceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .Include(x => x.ItensTabelaDePreco)
            .FirstOrDefaultAsync(x => x.AtivaEcommerce);
    }

    public async Task<TabelaDePreco?> GetTabelaDePrecoAtivaByProdutoIdAsync(Guid produtoId)
    {
        var tabelaDePreco = await ParceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AtivaEcommerce);

        if(tabelaDePreco != null)
        {
            tabelaDePreco.ItensTabelaDePreco = await ParceiroContext
                .ItensTabelaDePreco
                .Where(x => x.ProdutoId == produtoId)
                .ToListAsync();
        }

        return tabelaDePreco;
    }

    public async Task<TabelaDePreco?> GetTabelaDePrecoByIdAsync(Guid id)
    {
        var tabelaDePreco = await ParceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .AsSingleQuery()
            .Include(x => x.ItensTabelaDePreco)
                .ThenInclude(x => x.Produto)
            .FirstOrDefaultAsync(x => x.Id == id);
    
        if(tabelaDePreco != null)
        {
            foreach (var item in tabelaDePreco.ItensTabelaDePreco)
            {
                item.TabelaDePreco = null!;
                item.Produto.ItensTabelaDePreco = new();
            }
        }

        return tabelaDePreco;
    }

    public async Task<TabelaDePreco?> GetTabelaDePrecoByIdUpdateAsync(Guid id)
    {
        return await ParceiroContext
            .TabelaDePreco
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
