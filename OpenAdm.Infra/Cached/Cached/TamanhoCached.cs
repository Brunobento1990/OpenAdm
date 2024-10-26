using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class TamanhoCached : ITamanhoRepository
{
    private readonly TamanhoRepository _tamanhoRepository;
    private readonly ICachedService<Tamanho> _cachedService;
    private const string _keyList = "tamanhos";

    public TamanhoCached(TamanhoRepository tamanhoRepository, ICachedService<Tamanho> cachedService)
    {
        _tamanhoRepository = tamanhoRepository;
        _cachedService = cachedService;
    }

    public async Task<Tamanho> AddAsync(Tamanho entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _tamanhoRepository.AddAsync(entity);
    }

    public async Task<bool> DeleteAsync(Tamanho entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _tamanhoRepository.DeleteAsync(entity);
    }

    public async Task<PaginacaoViewModel<Tamanho>> GetPaginacaoTamanhoAsync(FilterModel<Tamanho> filterModel)
    {
        return await _tamanhoRepository.GetPaginacaoTamanhoAsync(filterModel);
    }

    public async Task<Tamanho?> GetTamanhoByIdAsync(Guid id)
    {
        var key = id.ToString();
        var tamanho = await _cachedService.GetItemAsync(key);

        if (tamanho == null)
        {
            tamanho = await _tamanhoRepository.GetTamanhoByIdAsync(id);

            if (tamanho != null)
            {
                await _cachedService.SetItemAsync(key, tamanho);
            }
        }

        return tamanho;
    }

    public async Task<IList<Tamanho>> GetTamanhosByIdsAsync(IList<Guid> ids)
    {
        return await _tamanhoRepository.GetTamanhosByIdsAsync(ids);
    }

    public async Task<Tamanho> UpdateAsync(Tamanho entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _tamanhoRepository.UpdateAsync(entity);
    }

    public async Task<IList<Tamanho>> GetTamanhosAsync()
    {
        var tamanhos = await _cachedService.GetListItemAsync(_keyList);

        if (tamanhos == null)
        {
            tamanhos = await _tamanhoRepository.GetTamanhosAsync();

            if (tamanhos != null)
            {
                await _cachedService.SetListItemAsync(_keyList, tamanhos);
            }
        }

        return tamanhos ?? new List<Tamanho>();
    }

    public async Task<IDictionary<Guid, string>> GetDescricaoTamanhosAsync(IList<Guid> ids)
    {
        return await _tamanhoRepository.GetDescricaoTamanhosAsync(ids);
    }

    public Task<PaginacaoViewModel<Tamanho>> PaginacaoAsync(FilterModel<Tamanho> filterModel)
        => _tamanhoRepository.PaginacaoAsync(filterModel);

    public Task<IDictionary<Guid, Tamanho>> GetDictionaryTamanhosAsync(IList<Guid> ids)
        => _tamanhoRepository.GetDictionaryTamanhosAsync(ids);
}
