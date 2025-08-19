using OpenAdm.Domain.Entities;
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

    public async Task AddRangeAsync(IList<ProdutoMaisVendido> produtosMaisVendidos)
    {
        if (produtosMaisVendidos.Count == 0) 
        {
            return;
        }
        await _parceiroContext.AddRangeAsync(produtosMaisVendidos);
    }

    public async Task<IList<ProdutoMaisVendido>> GetProdutosMaisVendidosAsync(IList<Guid> produtosIds)
    {
        return await _parceiroContext
            .ProdutosMaisVendidos
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();
    }

    public void UpdateRange(IList<ProdutoMaisVendido> produtosMaisVendidos)
    {
        if (produtosMaisVendidos.Count == 0)
        {
            return;
        }
        _parceiroContext.UpdateRange(produtosMaisVendidos);
    }
}
