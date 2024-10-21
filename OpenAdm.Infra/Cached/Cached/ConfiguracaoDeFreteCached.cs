using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class ConfiguracaoDeFreteCached : IConfiguracaoDeFreteRepository
{
    private const string KEY = "configuracao-frete";
    private readonly ICachedService<ConfiguracaoDeFrete> _cachedService;
    private readonly ConfiguracaoDeFreteRepository _configuracaoDeFreteRepository;

    public ConfiguracaoDeFreteCached(
        ICachedService<ConfiguracaoDeFrete> cachedService,
        ConfiguracaoDeFreteRepository configuracaoDeFreteRepository)
    {
        _cachedService = cachedService;
        _configuracaoDeFreteRepository = configuracaoDeFreteRepository;
    }

    public Task AddAsync(ConfiguracaoDeFrete configuracaoDeFrete)
        => _configuracaoDeFreteRepository.AddAsync(configuracaoDeFrete);

    public async Task<ConfiguracaoDeFrete?> GetAsync()
    {
        var configuracao = await _cachedService.GetItemAsync(KEY);

        if (configuracao == null)
        {
            configuracao = await _configuracaoDeFreteRepository.GetAsync();
            if (configuracao != null)
            {
                await _cachedService.SetItemAsync(KEY, configuracao);
            }
        }

        return configuracao;
    }

    public async Task UpdateAsync(ConfiguracaoDeFrete configuracaoDeFrete)
    {
        await _cachedService.RemoveCachedAsync(KEY);
        await _configuracaoDeFreteRepository.UpdateAsync(configuracaoDeFrete);
    }
}
