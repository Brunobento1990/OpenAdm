using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class EstoqueRepository : GenericRepository<Estoque>, IEstoqueRepository
{
    private readonly ParceiroContext _parceiroContext;
    public EstoqueRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
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
