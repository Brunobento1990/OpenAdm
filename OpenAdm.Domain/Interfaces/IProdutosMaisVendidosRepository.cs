using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IProdutosMaisVendidosRepository
{
    Task AddRangeAsync(IList<ProdutosMaisVendidos> produtosMaisVendidos);
    Task UpdateRangeAsync(IList<ProdutosMaisVendidos> produtosMaisVendidos);
    Task<IList<ProdutosMaisVendidos>> GetProdutosMaisVendidosAsync(IList<Guid> produtosIds);
}
