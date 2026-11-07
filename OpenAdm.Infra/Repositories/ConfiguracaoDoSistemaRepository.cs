using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Data.Context;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracaoDoSistemaRepository : GenericBaseRepository<ConfiguracaoDoSistema> ,IConfiguracaoDoSistemaRepository
{
    public ConfiguracaoDoSistemaRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<ConfiguracaoDoSistema?> GetConfiguracaoDoSistemaAsync(Guid parceiroId)
    {
        return await AppDbContext
            .ConfiguracaoDoSistema
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ativo && x.ParceiroId == parceiroId);
    }
}
