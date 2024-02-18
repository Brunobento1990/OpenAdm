using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Domain.Interfaces;

public interface ICarrinhoRepository
{
    Task<bool> AdicionarProdutoAsync(CarrinhoModel carrinhoModel, string key);
    Task<CarrinhoModel> GetCarrinhoAsync(string key);
    Task<int> GetCountCarrinhoAsync(string key);
    Task DeleteCarrinhoAsync(string key);
}
