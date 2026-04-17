using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracaoDeFreteRepository : GenericBaseRepository<ConfiguracaoDeFrete>, IConfiguracaoDeFreteRepository
{
    public ConfiguracaoDeFreteRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<ConfiguracaoDeFrete?> ObterDoParceiroAsync(Guid parceiroId)
    {
        return await AppDbContext
            .ConfiguracoesDeFrete
            .FirstOrDefaultAsync(x => x.ParceiroId == parceiroId);
    }
}