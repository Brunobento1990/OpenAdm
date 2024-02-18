﻿using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class TabelaDePrecoCached : ITabelaDePrecoRepository
{
    private readonly TabelaDePrecoRepository _tabelaDePrecoRepository;
    private readonly ICachedService<TabelaDePreco> _cachedService;
    private const string _keyList = "tabelas-de-precos";
    private const string _keyTabelaAtiva = "tabela-de-preco";

    public TabelaDePrecoCached(TabelaDePrecoRepository tabelaDePrecoRepository, ICachedService<TabelaDePreco> cachedService)
    {
        _tabelaDePrecoRepository = tabelaDePrecoRepository;
        _cachedService = cachedService;
    }

    public async Task<TabelaDePreco> AddAsync(TabelaDePreco entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(_keyTabelaAtiva);
        return await _tabelaDePrecoRepository.AddAsync(entity);
    }

    public async Task<bool> DeleteAsync(TabelaDePreco entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(_keyTabelaAtiva);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _tabelaDePrecoRepository.DeleteAsync(entity);
    }

    public async Task<TabelaDePreco?> GetTabelaDePrecoAtivaAsync()
    {
        var tabelaDePreco = await _cachedService.GetItemAsync(_keyTabelaAtiva);

        if(tabelaDePreco == null)
        {
            tabelaDePreco = await _tabelaDePrecoRepository.GetTabelaDePrecoAtivaAsync();
            if(tabelaDePreco != null)
            {
                tabelaDePreco.ItensTabelaDePreco.ForEach(item =>
                {
                    if (item.TabelaDePreco != null)
                    {
                        item.TabelaDePreco = null;
                    }
                });

                await _cachedService.SetItemAsync(_keyTabelaAtiva, tabelaDePreco);
            }
        }

        return tabelaDePreco;
    }

    public async Task<TabelaDePreco> UpdateAsync(TabelaDePreco entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(_keyTabelaAtiva);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _tabelaDePrecoRepository.UpdateAsync(entity);
    }
}