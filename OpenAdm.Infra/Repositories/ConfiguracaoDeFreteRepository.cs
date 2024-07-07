using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ConfiguracaoDeFreteRepository : GenericRepository<ConfiguracaoDeFrete>, IConfiguracaoDeFreteRepository
{
    private readonly ParceiroContext _parceiroContext;
    public ConfiguracaoDeFreteRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<ConfiguracaoDeFrete?> GetConfiguracaoAsync()
    {
        return await _parceiroContext
            .ConfiguracoesDeFrete
            .AsNoTracking()
            .OrderBy(x => x.DataDeCriacao)
            .FirstOrDefaultAsync();
    }
}
