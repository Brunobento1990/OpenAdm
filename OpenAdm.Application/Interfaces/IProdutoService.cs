using OpenAdm.Application.Models.Produtos;
using OpenAdm.Application.PaginateDto;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IProdutoService
{
    Task<PaginacaoViewModel<ProdutoViewModel>> GetProdutosAsync(int page);
    Task<IList<ProdutoViewModel>> GetProdutosByCategoriaIdAsync(Guid categoriaId);
    Task<PaginacaoViewModel<ProdutoViewModel>> GetPaginacaoAsync(PaginacaoProdutoDto paginacaoProdutoDto);
}
