using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Domain.Interfaces;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    Task<IList<Produto>> GetProdutosMaisVendidosAsync();
    Task<IList<Produto>> GetAllProdutosAsync();
    Task<IList<Produto>> GetProdutosByCategoriaIdAsync(Guid categoriaId);
    Task<IList<Produto>> GetProdutosByListIdAsync(List<Guid> ids);
    Task<PaginacaoViewModel<Produto>> GetProdutosAsync(PaginacaoProdutoEcommerceDto paginacaoProdutoEcommerceDto);
    Task<PaginacaoViewModel<Produto>> GetPaginacaoProdutoAsync(FilterModel<Produto> filterModel);
    Task<Produto?> GetProdutoByIdAsync(Guid id);
    Task<IDictionary<Guid, string>> GetDescricaoDeProdutosAsync(IList<Guid> ids);
}
