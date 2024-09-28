using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IMovimentacaoDeProdutoRepository : IGenericRepository<MovimentacaoDeProduto>
{
    Task<PaginacaoViewModel<MovimentacaoDeProduto>>
        GetPaginacaoMovimentacaoDeProdutoAsync(FilterModel<MovimentacaoDeProduto> filterModel);
    Task AddRangeAsync(IList<MovimentacaoDeProduto> movimentacaoDeProdutos);
    Task<IDictionary<int, decimal>> CountTresMesesAsync();
    Task<IList<MovimentacaoDeProduto>> RelatorioAsync(DateTime dataInicial, DateTime dataFinal, IList<Guid> produtosIds, IList<Guid> pesosIds, IList<Guid> tamanhosIds);
}
