using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Worker.Infra.Repositories;

public class ConfiguracaoDoSistemaRepository : IConfiguracaoDoSistemaRepository
{
    private readonly AppDbContext _appDbContext;

    public ConfiguracaoDoSistemaRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task AddAsync(ConfiguracaoDoSistema entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(ConfiguracaoDoSistema entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ConfiguracaoDoSistema?> GetConfiguracaoDoSistemaAsync(Guid parceiroId)
    {
        return await _appDbContext
            .ConfiguracaoDoSistema
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .FirstOrDefaultAsync(x => x.Ativo && x.ParceiroId == parceiroId);
    }

    public Task<PaginacaoViewModel<ConfiguracaoDoSistema>> PaginacaoAsync(FilterModel<ConfiguracaoDoSistema> filterModel)
    {
        throw new NotImplementedException();
    }

    public Task<long> ProximoNumeroAsync(Guid parceiroId)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public void Update(ConfiguracaoDoSistema entity)
    {
        throw new NotImplementedException();
    }
}
