using OpenAdm.Application.Models.Carrinhos;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Application.Interfaces;

public interface ICarrinhoService
{
    Task<bool> AdicionarProdutoAsync(IList<AddCarrinhoModel> addCarrinhoModel);
    Task<IList<CarrinhoViewModel>> GetCarrinhoAsync();
    Task<int> GetCountCarrinhoAsync();
    Task<bool> DeleteProdutoCarrinhoAsync(Guid produtoId);
    Task LimparCarrinhoDoUsuarioAsync(Guid usuarioId);
}
