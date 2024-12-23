using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracaoDeEmailRepository : GenericRepository<ConfiguracaoDeEmail>, IConfiguracaoDeEmailRepository
{
    public ConfiguracaoDeEmailRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<ConfiguracaoDeEmail?> GetConfiguracaoDeEmailAtivaAsync()
    {
        return await ParceiroContext
            .ConfiguracoesDeEmail
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ativo);
    }
}
