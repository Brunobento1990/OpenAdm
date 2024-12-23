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

    public async Task<Estoque?> GetEstoqueAsync(Expression<Func<Estoque, bool>> where)
    {
        return await ParceiroContext
            .Estoques
            .AsNoTracking()
            .FirstOrDefaultAsync(where);
    }

    public async Task<Estoque?> GetEstoqueByProdutoIdAndPesoIdAsync(Guid produtoId, Guid pesoId)
    {
        return await ParceiroContext
            .Estoques
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.PesoId == pesoId);
    }

    public async Task<Estoque?> GetEstoqueByProdutoIdAndTamanhoIdAsync(Guid produtoId, Guid tamanhoId)
    {
        return await ParceiroContext
            .Estoques
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.TamanhoId == tamanhoId);
    }

    public async Task<Estoque?> GetEstoqueByProdutoIdAsync(Guid produtoId)
    {
        return await ParceiroContext
            .Estoques
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProdutoId == produtoId);
    }
}
