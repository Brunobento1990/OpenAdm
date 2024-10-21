using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public sealed class ConfiguracaoDePagamentoCached : IConfiguracaoDePagamentoRepository
{
    private const string KEY = "configuracao-de-pagamento";
    private readonly ConfiguracaoDePagamentoRepository _configuracaoDePagamentoRepository;
    private readonly ICachedService<ConfiguracaoDePagamento> _cachedService;

    public ConfiguracaoDePagamentoCached(ConfiguracaoDePagamentoRepository configuracaoDePagamentoRepository, ICachedService<ConfiguracaoDePagamento> cachedService)
    {
        _configuracaoDePagamentoRepository = configuracaoDePagamentoRepository;
        _cachedService = cachedService;
    }

    public Task AddAsync(ConfiguracaoDePagamento configuracaoDePagamento)
        => _configuracaoDePagamentoRepository.AddAsync(configuracaoDePagamento);

    public async Task<ConfiguracaoDePagamento?> GetAsync()
    {
        var configuracao = await _cachedService.GetItemAsync(KEY);

        if (configuracao == null)
        {
            configuracao = await _configuracaoDePagamentoRepository.GetAsync();
            if (configuracao != null)
            {
                await _cachedService.SetItemAsync(KEY, configuracao);
            }
        }

        return configuracao;
    }

    public async Task UpdateAsync(ConfiguracaoDePagamento configuracaoDePagamento)
    {
        await _cachedService.RemoveCachedAsync(KEY);
        await _configuracaoDePagamentoRepository.UpdateAsync(configuracaoDePagamento);
    }
}
