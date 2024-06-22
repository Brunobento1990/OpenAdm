using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracaoParceiroRepository
    : IConfiguracaoParceiroRepository
{
    private readonly OpenAdmContext _context;
    
    public ConfiguracaoParceiroRepository(
        OpenAdmContext context)
    {
        _context = context;
    }

    public async Task<ConfiguracaoParceiro?> GetParceiroByDominioAdmAsync(string dominio)
    {
        return await _context
            .ConfiguracoesParceiro
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DominioSiteAdm == dominio);
    }

    public async Task<ConfiguracaoParceiro?> GetParceiroByXApiAsync(Guid xApi)
    {
        return await _context
            .ConfiguracoesParceiro
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.XApi == xApi);
    }
}
