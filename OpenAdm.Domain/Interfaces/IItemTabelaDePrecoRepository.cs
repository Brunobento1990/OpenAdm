using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IItemTabelaDePrecoRepository : IGenericRepository<ItensTabelaDePreco>
{
    Task<ItensTabelaDePreco?> GetItemTabelaDePrecoByIdAsync(Guid id);
}
