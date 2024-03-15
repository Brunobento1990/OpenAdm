using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface ITabelaDePrecoRepository : IGenericRepository<TabelaDePreco>
{
    Task<TabelaDePreco?> GetTabelaDePrecoAtivaAsync();
    Task<TabelaDePreco?> GetTabelaDePrecoAtivaByProdutoIdAsync(Guid produtoId);
    Task<TabelaDePreco?> GetTabelaDePrecoByIdAsync(Guid id);
    Task<TabelaDePreco?> GetTabelaDePrecoByIdUpdateAsync(Guid id);
    Task<PaginacaoViewModel<TabelaDePreco>> GetPaginacaoAsync(FilterModel<TabelaDePreco> filterModel);
    Task<int> GetCountTabelaDePrecoAsync();
    Task<IList<TabelaDePreco>> GetAllTabelaDePrecoAsync();
}
