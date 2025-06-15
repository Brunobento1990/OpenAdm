using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IMovimentacaoDeProdutoRepository : IGenericRepository<MovimentacaoDeProduto>
{
    Task AddRangeAsync(IList<MovimentacaoDeProduto> movimentacaoDeProdutos);
    Task<IDictionary<int, List<MovimentacaoDeProduto>>> CountTresMesesAsync(DateTime dataInicio, DateTime dataFinal);
    Task<IList<MovimentacaoDeProduto>> RelatorioAsync(DateTime dataInicial, DateTime dataFinal, IList<Guid> produtosIds, IList<Guid> pesosIds, IList<Guid> tamanhosIds);
}
