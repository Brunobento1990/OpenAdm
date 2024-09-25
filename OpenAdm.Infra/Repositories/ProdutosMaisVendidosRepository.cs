using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ProdutosMaisVendidosRepository : IProdutosMaisVendidosRepository
{
    private readonly ParceiroContext _parceiroContext;

    public ProdutosMaisVendidosRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task AddRangeAsync(IList<ProdutosMaisVendidos> produtosMaisVendidos)
    {
        if (produtosMaisVendidos.Count == 0) 
        {
            return;
        }
        await _parceiroContext.AddRangeAsync(produtosMaisVendidos);
        await _parceiroContext.SaveChangesAsync();
    }

    public async Task<IList<ProdutosMaisVendidos>> GetProdutosMaisVendidosAsync(IList<Guid> produtosIds)
    {
        return await _parceiroContext
            .ProdutosMaisVendidos
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();
    }

    public async Task UpdateRangeAsync(IList<ProdutosMaisVendidos> produtosMaisVendidos)
    {
        _parceiroContext.AttachRange(produtosMaisVendidos);
        _parceiroContext.UpdateRange(produtosMaisVendidos);
        await _parceiroContext.SaveChangesAsync();
    }
}
