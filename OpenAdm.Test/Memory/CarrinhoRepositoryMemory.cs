using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Test.Memory;

public class CarrinhoRepositoryMemory : ICarrinhoRepository
{
    private IDictionary<string, CarrinhoModel> _carrinho;

    public CarrinhoRepositoryMemory()
    {
        _carrinho = new Dictionary<string, CarrinhoModel>();
    }

    public async Task<bool> AdicionarProdutoAsync(CarrinhoModel carrinhoModel, string key)
    {
        var carrinho = _carrinho.FirstOrDefault(x => x.Key == key).Value;
        if(carrinho == null)
        {
            _carrinho.Add(key, carrinhoModel);
        }
        else
        {
            carrinho.Produtos.AddRange(carrinhoModel.Produtos);
        }

        return await Task.FromResult(true);
    }

    public async Task DeleteCarrinhoAsync(string key)
    {
        await Task.FromResult(_carrinho?.Remove(key));
    }

    public async Task<CarrinhoModel> GetCarrinhoAsync(string key)
    {
        var carrinho = await Task.FromResult(_carrinho.FirstOrDefault(x => x.Key == key).Value);
        return carrinho ?? new CarrinhoModel();
    }

    public async Task<int> GetCountCarrinhoAsync(string key)
    {
        var carrinho = await Task.FromResult(_carrinho.FirstOrDefault(x => x.Key == key).Value);
        return carrinho.Produtos?.Count ?? 0;
    }
}
