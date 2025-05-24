using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public class EmpresaOpenAdmCached : IEmpresaOpenAdmRepository
{
    private readonly ICachedService<EmpresaOpenAdm> _cachedService;
    private readonly EmpresaOpenAdmRepository _empresaOpenAdmRepository;

    public EmpresaOpenAdmCached(ICachedService<EmpresaOpenAdm> cachedService, EmpresaOpenAdmRepository empresaOpenAdmRepository)
    {
        _cachedService = cachedService;
        _empresaOpenAdmRepository = empresaOpenAdmRepository;
    }

    public async Task<EmpresaOpenAdm?> ObterPorOrigemAsync(string origem)
    {
        var empresa = await _cachedService.GetItemAsync(origem);

        if (empresa == null)
        {
            empresa = await _empresaOpenAdmRepository.ObterPorOrigemAsync(origem);
            if (empresa != null)
            {
                await _cachedService.SetItemAsync(origem, empresa);
            }
        }

        return empresa;
    }
}
