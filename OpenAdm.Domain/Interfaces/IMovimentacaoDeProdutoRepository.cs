using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IMovimentacaoDeProdutoRepository : IGenericRepository<MovimentacaoDeProduto>
{
    Task<PaginacaoViewModel<MovimentacaoDeProduto>>
        GetPaginacaoMovimentacaoDeProdutoAsync(FilterModel<MovimentacaoDeProduto> filterModel);
    Task AddRangeAsync(IList<MovimentacaoDeProduto> movimentacaoDeProdutos);
    Task<IDictionary<int, decimal>> CountTresMesesAsync();
}
