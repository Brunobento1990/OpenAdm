using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IMovimentacaoDeProdutoRepository : IGenericRepository<MovimentacaoDeProduto>
{
    Task<PaginacaoViewModel<MovimentacaoDeProduto>>
        GetPaginacaoMovimentacaoDeProdutoAsync(FilterModel<MovimentacaoDeProduto> filterModel);
}
