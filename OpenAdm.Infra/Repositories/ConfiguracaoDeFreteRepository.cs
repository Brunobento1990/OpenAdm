using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ConfiguracaoDeFreteRepository : IConfiguracaoDeFreteRepository
{
    private readonly ParceiroContext _parceiroContext;

    public ConfiguracaoDeFreteRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task AddAsync(ConfiguracaoDeFrete configuracaoDeFrete)
    {
        await _parceiroContext.AddAsync(configuracaoDeFrete);
        await _parceiroContext.SaveChangesAsync();
    }

    public async Task<ConfiguracaoDeFrete?> GetAsync()
    {
        return await _parceiroContext
            .ConfiguracoesDeFrete
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(ConfiguracaoDeFrete configuracaoDeFrete)
    {
        _parceiroContext.Attach(configuracaoDeFrete);
        _parceiroContext.Update(configuracaoDeFrete);
        await _parceiroContext.SaveChangesAsync();
    }
}
