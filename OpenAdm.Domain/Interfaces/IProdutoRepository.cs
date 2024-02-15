using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    Task<IList<Produto>> GetProdutosMaisVendidosAsync();
}
