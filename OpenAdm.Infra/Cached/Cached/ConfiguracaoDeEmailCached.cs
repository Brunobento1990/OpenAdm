using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Infra.Cached.Cached;

public class ConfiguracaoDeEmailCached(
    ConfiguracaoDeEmailRepository configuracaoDeEmailRepository,
    ICachedService<ConfiguracaoDeEmail> cachedService) : IConfiguracaoDeEmailRepository
{
    private readonly ConfiguracaoDeEmailRepository _configuracaoDeEmailRepository = configuracaoDeEmailRepository;
    private readonly ICachedService<ConfiguracaoDeEmail> _cachedService = cachedService;
    private const string _keyList = "configuracoes-email";

    public async Task<ConfiguracaoDeEmail> AddAsync(ConfiguracaoDeEmail entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _configuracaoDeEmailRepository.AddAsync(entity);
    }

    public async Task<bool> DeleteAsync(ConfiguracaoDeEmail entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _configuracaoDeEmailRepository.DeleteAsync(entity);
    }

    public async Task<ConfiguracaoDeEmail?> GetConfiguracaoDeEmailAtivaAsync()
    {
        var key = $"um-{_keyList}";

        var configuracao = await _cachedService.GetItemAsync(key);

        if (configuracao == null)
        {
            configuracao = await _configuracaoDeEmailRepository.GetConfiguracaoDeEmailAtivaAsync();

            if (configuracao != null)
            {
                await _cachedService.SetItemAsync(key, configuracao);
            }
        }

        return configuracao;
    }

    public async Task<ConfiguracaoDeEmail> UpdateAsync(ConfiguracaoDeEmail entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _configuracaoDeEmailRepository.UpdateAsync(entity);
    }

    public Task<PaginacaoViewModel<ConfiguracaoDeEmail>> PaginacaoAsync(FilterModel<ConfiguracaoDeEmail> filterModel)
        => _configuracaoDeEmailRepository.PaginacaoAsync(filterModel);

    public Task<IList<ConfiguracaoDeEmail>> PaginacaoDropDownAsync(PaginacaoDropDown<ConfiguracaoDeEmail> paginacaoDropDown)
        => _configuracaoDeEmailRepository.PaginacaoDropDownAsync(paginacaoDropDown);

    public Task<int> SaveChangesAsync()
        => _configuracaoDeEmailRepository.SaveChangesAsync();
}
