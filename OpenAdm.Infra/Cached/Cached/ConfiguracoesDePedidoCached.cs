using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class ConfiguracoesDePedidoCached(
    ConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
    ICachedService<ConfiguracoesDePedido> cachedService) : IConfiguracoesDePedidoRepository
{
    private const string KeyPrefix = "configuracoes-de-pedido";
    private readonly HashSet<string> _keysParaRemover = [];

    public async Task<ConfiguracoesDePedido?> GetConfiguracoesDePedidoAsync(Guid parceiroId)
    {
        var key = ObterKey(parceiroId);
        var configuracoes = await cachedService.GetItemAsync(key);

        if (configuracoes is not null)
        {
            return configuracoes;
        }

        configuracoes = await configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(parceiroId);

        if (configuracoes is not null)
        {
            await cachedService.SetItemAsync(key, configuracoes);
        }

        return configuracoes;
    }

    public async Task AddAsync(ConfiguracoesDePedido entity)
    {
        await configuracoesDePedidoRepository.AddAsync(entity);
    }

    public void Update(ConfiguracoesDePedido entity)
    {
        MarcarCacheParaRemocao(entity.ParceiroId);
        configuracoesDePedidoRepository.Update(entity);
    }

    public void Delete(ConfiguracoesDePedido entity)
    {
        MarcarCacheParaRemocao(entity.ParceiroId);
        configuracoesDePedidoRepository.Delete(entity);
    }

    public async Task SaveChangesAsync()
    {
        foreach (var key in _keysParaRemover)
        {
            await cachedService.RemoveCachedAsync(key);
        }

        _keysParaRemover.Clear();
        await configuracoesDePedidoRepository.SaveChangesAsync();
    }

    public Task<long> ProximoNumeroAsync(Guid parceiroId)
        => configuracoesDePedidoRepository.ProximoNumeroAsync(parceiroId);

    public Task<PaginacaoViewModel<ConfiguracoesDePedido>> PaginacaoAsync(
        FilterModel<ConfiguracoesDePedido> filterModel)
        => configuracoesDePedidoRepository.PaginacaoAsync(filterModel);

    private void MarcarCacheParaRemocao(Guid parceiroId)
        => _keysParaRemover.Add(ObterKey(parceiroId));

    private static string ObterKey(Guid parceiroId)
        => $"{KeyPrefix}_{parceiroId}";
}
