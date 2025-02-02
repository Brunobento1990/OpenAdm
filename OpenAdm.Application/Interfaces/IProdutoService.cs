using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface IProdutoService
{
    Task<PaginacaoViewModel<ProdutoViewModel>> GetProdutosAsync(PaginacaoProdutoEcommerceDto paginacaoProdutoEcommerceDto);
    Task<IList<ProdutoViewModel>> GetProdutosByCategoriaIdAsync(Guid categoriaId);
    Task<IList<ProdutoViewModel>> GetAllProdutosAsync();
    Task<PaginacaoViewModel<ProdutoViewModel>> GetPaginacaoAsync(FilterModel<Produto> paginacaoProdutoDto);
    Task<ProdutoViewModel> CreateProdutoAsync(CreateProdutoDto createProdutoDto);
    Task<ProdutoViewModel> GetProdutoViewModelByIdAsync(Guid id);
    Task DeleteProdutoAsync(Guid id);
    Task<ProdutoViewModel> UpdateProdutoAsync(UpdateProdutoDto updateProdutoDto);
}
