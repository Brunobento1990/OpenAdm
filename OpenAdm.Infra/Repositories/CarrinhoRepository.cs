using Microsoft.Extensions.Caching.Distributed;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;
using System.Text.Json;

namespace OpenAdm.Infra.Repositories;

public class CarrinhoRepository : ICarrinhoRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _options;
    private static readonly double _absolutExpiration = 24;
    private static readonly double _slidingExpiration = 12;

    public CarrinhoRepository(IDistributedCache distributedCache)
    {
        _options = new DistributedCacheEntryOptions()
                      .SetAbsoluteExpiration(TimeSpan.FromHours(_absolutExpiration))
                      .SetSlidingExpiration(TimeSpan.FromHours(_slidingExpiration));

        _distributedCache = distributedCache;
    }

    public async Task<bool> AdicionarProdutoAsync(CarrinhoModel carrinhoModel, string key)
    {
        await _distributedCache
        .SetStringAsync(key, JsonSerializer.Serialize(carrinhoModel), _options);

        return true;
    }

    public async Task<CarrinhoModel> GetCarrinhoAsync(string key)
    {
        var carrinhoString = await _distributedCache.GetStringAsync(key);

        var carrinho = carrinhoString == null ?
            new CarrinhoModel() :
            JsonSerializer.Deserialize<CarrinhoModel>(carrinhoString);

        return carrinho ?? new();
    }

    public async Task<int> GetCountCarrinhoAsync(string key)
    {
        var carrinho = await GetCarrinhoAsync(key);

        if (carrinho == null) return 0;

        return carrinho.Produtos.Select(x => x.ProdutoId).ToList().Count;
    }
}
