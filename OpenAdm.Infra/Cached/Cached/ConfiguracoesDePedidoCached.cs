using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class ConfiguracoesDePedidoCached : IConfiguracoesDePedidoRepository
{
    private readonly ConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly ICachedService<ConfiguracoesDePedido> _cachedService;
    private const string _key = "configuracao-de-pedido";

    public ConfiguracoesDePedidoCached(
        ConfiguracoesDePedidoRepository configuracoesDePedidoRepository, 
        ICachedService<ConfiguracoesDePedido> cachedService)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _cachedService = cachedService;
    }

    public async Task<ConfiguracoesDePedido> AddAsync(ConfiguracoesDePedido entity)
    {
        return await _configuracoesDePedidoRepository.AddAsync(entity);
    }

    public async Task<bool> DeleteAsync(ConfiguracoesDePedido entity)
    {
        await _cachedService.RemoveCachedAsync(_key);
        return await _configuracoesDePedidoRepository.DeleteAsync(entity);
    }

    public async Task<ConfiguracoesDePedido?> GetConfiguracoesDePedidoAsync()
    {
        var configuracao = await _cachedService.GetItemAsync(_key);

        if(configuracao == null)
        {
            configuracao = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync();

            if(configuracao != null)
            {
                await _cachedService.SetItemAsync(_key, configuracao);
            }
        }

        return configuracao;
    }

    public async Task<ConfiguracoesDePedido> UpdateAsync(ConfiguracoesDePedido entity)
    {
        await _cachedService.RemoveCachedAsync(_key);
        return await _configuracoesDePedidoRepository.UpdateAsync(entity);
    }
}
