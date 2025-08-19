using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Repositories;

public class EstoqueRepository : GenericRepository<Estoque>, IEstoqueRepository
{
    public EstoqueRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task AddRangeAsync(IList<Estoque> entities)
    {
        await ParceiroContext.AddRangeAsync(entities);
    }

    public async Task<Estoque?> GetEstoqueAsync(Expression<Func<Estoque, bool>> where)
    {
        return await ParceiroContext
            .Estoques
            .Include(x => x.Produto)
            .Include(x => x.Tamanho)
            .Include(x => x.Peso)
            .FirstOrDefaultAsync(where);
    }

    public async Task<IList<Estoque>> GetPosicaoEstoqueAsync()
    {
        return await ParceiroContext
            .Estoques
            .OrderByDescending(x => x.DataDeAtualizacao)
            .Take(3)
            .ToListAsync();
    }

    public async Task<IList<Estoque>> GetPosicaoEstoqueDosProdutosAsync(IList<Guid> produtosIds)
    {
        return await ParceiroContext
            .Estoques
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .OrderByDescending(x => x.DataDeAtualizacao)
            .ToListAsync();
    }

    public void UpdateRange(IList<Estoque> entities)
    {
        ParceiroContext.UpdateRange(entities);
    }
}
