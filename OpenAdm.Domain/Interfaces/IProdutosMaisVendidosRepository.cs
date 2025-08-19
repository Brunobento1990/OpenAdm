using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IProdutosMaisVendidosRepository
{
    Task AddRangeAsync(IList<ProdutoMaisVendido> produtosMaisVendidos);
    void UpdateRange(IList<ProdutoMaisVendido> produtosMaisVendidos);
    Task<IList<ProdutoMaisVendido>> GetProdutosMaisVendidosAsync(IList<Guid> produtosIds);
}
