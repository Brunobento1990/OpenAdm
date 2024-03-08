using Domain.Pkg.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class PesoCached : IPesoRepository
{
    private readonly PesoRepository _pesoRepository;
    private readonly ICachedService<Peso> _cachedService;
    private const string _keyList = "pesos";

    public PesoCached(PesoRepository pesoRepository, ICachedService<Peso> cachedService)
    {
        _pesoRepository = pesoRepository;
        _cachedService = cachedService;
    }

    public async Task<Peso> AddAsync(Peso entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _pesoRepository.AddAsync(entity);
    }

    public async Task<bool> DeleteAsync(Peso entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _pesoRepository.DeleteAsync(entity);
    }

    public async Task<PaginacaoViewModel<Peso>> GetPaginacaoPesoAsync(FilterModel<Peso> filterModel)
    {
        return await _pesoRepository.GetPaginacaoPesoAsync(filterModel);
    }

    public async Task<IList<Peso>> GetPesosByIdsAsync(IList<Guid> ids)
    {
        return await _pesoRepository.GetPesosByIdsAsync(ids);
    }

    public async Task<Peso> UpdateAsync(Peso entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _pesoRepository.UpdateAsync(entity);
    }

    public async Task<Peso?> GetPesoByIdAsync(Guid id)
    {
        var key = id.ToString();
        var peso = await _cachedService.GetItemAsync(key);

        if (peso == null)
        {
            peso = await _pesoRepository.GetPesoByIdAsync(id);

            if (peso != null)
            {
                await _cachedService.SetItemAsync(key, peso);
            }
        }

        return peso;
    }

    public async Task<IList<Peso>> GetPesosAsync()
    {
        var pesos = await _cachedService.GetListItemAsync(_keyList);

        if(pesos == null)
        {
            pesos = await _pesoRepository.GetPesosAsync();

            if(pesos != null)
            {
                await _cachedService.SetListItemAsync(_keyList, pesos);
            }
        }

        return pesos ?? new List<Peso>();
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoPesosAsync(IList<Guid> ids)
    {
        return await _pesoRepository.GetDescricaoPesosAsync(ids);
    }
}
