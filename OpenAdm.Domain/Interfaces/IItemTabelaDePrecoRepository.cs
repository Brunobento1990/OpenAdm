using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IItemTabelaDePrecoRepository : IGenericRepository<ItensTabelaDePreco>
{
    Task<ItensTabelaDePreco?> GetItemTabelaDePrecoByIdAsync(Guid id);
    Task<IList<ItensTabelaDePreco>> GetItensTabelaDePrecoByIdProdutosAsync(IList<Guid> produtosIds);
    Task AddRangeAsync(IList<ItensTabelaDePreco> itensTabelaDePrecos);
    Task DeleteItensTabelaDePrecoByProdutoIdAsync(Guid produtoId);
}
