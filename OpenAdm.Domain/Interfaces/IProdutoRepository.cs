using OpenAdm.Domain.Entities;
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
    Task<Produto?> GetProdutoByIdAsync(Guid id);
    Task<Produto?> GetProdutoByIdParaEditarAsync(Guid id);
    Task<IDictionary<Guid, string>> GetDescricaoDeProdutosAsync(IList<Guid> ids);
    Task<IDictionary<Guid, Produto>> GetDictionaryProdutosAsync(IList<Guid> ids);
}
