using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Repositories;

public class EstoqueRepository : GenericRepository<Estoque>, IEstoqueRepository
{
    private readonly ParceiroContext _parceiroContext;
    public EstoqueRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<Estoque?> GetEstoqueAsync(Expression<Func<Estoque, bool>> where)
    {
        return await _parceiroContext
            .Estoques
            .AsNoTracking()
            .FirstOrDefaultAsync(where);
    }

    public async Task<Estoque?> GetEstoqueByProdutoIdAndPesoIdAsync(Guid produtoId, Guid pesoId)
    {
        return await _parceiroContext
            .Estoques
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.PesoId == pesoId);
    }

    public async Task<Estoque?> GetEstoqueByProdutoIdAndTamanhoIdAsync(Guid produtoId, Guid tamanhoId)
    {
        return await _parceiroContext
            .Estoques
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.TamanhoId == tamanhoId);
    }

    public async Task<Estoque?> GetEstoqueByProdutoIdAsync(Guid produtoId)
    {
        return await _parceiroContext
            .Estoques
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProdutoId == produtoId);
    }

    public async Task<PaginacaoViewModel<Estoque>> GetPaginacaoEstoqueAsync(FilterModel<Estoque> filterModel)
    {
        var (total, values) = await _parceiroContext
                .Estoques
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Estoque>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}
