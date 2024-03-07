using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class MovimentacaoDeProdutoRepository : GenericRepository<MovimentacaoDeProduto>, IMovimentacaoDeProdutoRepository
{
    private readonly ParceiroContext _parceiroContext;
    public MovimentacaoDeProdutoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<PaginacaoViewModel<MovimentacaoDeProduto>>
        GetPaginacaoMovimentacaoDeProdutoAsync(FilterModel<MovimentacaoDeProduto> filterModel)
    {
        var (total, values) = await _parceiroContext
                .MovimentacoesDeProdutos
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<MovimentacaoDeProduto>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}
