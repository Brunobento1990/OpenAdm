using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface ITabelaDePrecoRepository : IGenericRepository<TabelaDePreco>
{
    Task<TabelaDePreco?> GetTabelaDePrecoAtivaAsync();
    Task<PaginacaoViewModel<TabelaDePreco>> GetPaginacaoAsync(FilterModel<TabelaDePreco> filterModel);
}
