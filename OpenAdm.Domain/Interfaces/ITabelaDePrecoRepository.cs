using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITabelaDePrecoRepository : IGenericRepository<TabelaDePreco>
{
    Task<TabelaDePreco?> GetTabelaDePrecoAtivaAsync();
    Task<TabelaDePreco?> GetTabelaDePrecoAtivaByProdutoIdAsync(Guid produtoId);
    Task<TabelaDePreco?> GetTabelaDePrecoByIdAsync(Guid id);
    Task<TabelaDePreco?> GetTabelaDePrecoByIdUpdateAsync(Guid id);
    Task<int> GetCountTabelaDePrecoAsync();
    Task<IList<TabelaDePreco>> GetAllTabelaDePrecoAsync();
}
