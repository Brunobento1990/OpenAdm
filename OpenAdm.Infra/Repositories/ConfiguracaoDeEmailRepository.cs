using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracaoDeEmailRepository : GenericRepository<ConfiguracaoDeEmail>, IConfiguracaoDeEmailRepository
{
    private readonly ParceiroContext _parceiroContext;
    public ConfiguracaoDeEmailRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<ConfiguracaoDeEmail?> GetConfiguracaoDeEmailAtivaAsync()
    {
        return await _parceiroContext
            .ConfiguracoesDeEmail
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ativo);
    }
}
