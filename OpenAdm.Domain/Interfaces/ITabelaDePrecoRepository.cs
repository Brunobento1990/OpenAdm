using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITabelaDePrecoRepository : IGenericRepository<TabelaDePreco>
{
    Task<TabelaDePreco?> GetTabelaDePrecoAtivaAsync();
}
