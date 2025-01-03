﻿using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public sealed class ConfiguracaoParceiroCached : IConfiguracaoParceiroRepository
{
    private readonly ConfiguracaoParceiroRepository _configuracaoParceiroRepository;
    private readonly ICachedService<ConfiguracaoParceiro> _cachedService;

    public ConfiguracaoParceiroCached(
        ConfiguracaoParceiroRepository configuracaoParceiroRepository,
        ICachedService<ConfiguracaoParceiro> cachedService)
    {
        _configuracaoParceiroRepository = configuracaoParceiroRepository;
        _cachedService = cachedService;
    }

    public Task<ConfiguracaoParceiro?> GetParceiroAdmByMercadoPagoAsync(string cliente)
        => _configuracaoParceiroRepository.GetParceiroAdmByMercadoPagoAsync(cliente);

    public Task<ConfiguracaoParceiro?> GetParceiroAutenticadoAdmAsync()
        => _configuracaoParceiroRepository.GetParceiroAutenticadoAdmAsync();

    public async Task<ConfiguracaoParceiro?> GetParceiroByDominioAdmAsync(string dominio)
    {
        var configuracaoParceiro = await _cachedService.GetItemAsync(dominio);

        if (configuracaoParceiro == null)
        {
            configuracaoParceiro = await _configuracaoParceiroRepository.GetParceiroByDominioAdmAsync(dominio);
            if (configuracaoParceiro != null)
            {
                if (configuracaoParceiro.Parceiro != null)
                {
                    configuracaoParceiro.Parceiro.ConfiguracaoDbParceiro = null!;
                }
                await _cachedService.SetItemAsync(dominio, configuracaoParceiro);
            }
        }

        return configuracaoParceiro;
    }
}
