using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Worker.Infra.Repositories;

public class ConfiguracoesDePedidoRepository : IConfiguracoesDePedidoRepository
{
    private readonly AppDbContext _appDbContext;

    public ConfiguracoesDePedidoRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task AddAsync(ConfiguracoesDePedido entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(ConfiguracoesDePedido entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ConfiguracoesDePedido?> GetConfiguracoesDePedidoAsync(Guid parceiroId)
    {
        return await _appDbContext
            .ConfiguracoesDePedidos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .FirstOrDefaultAsync(x => x.Ativo && x.ParceiroId == parceiroId);
    }

    public Task<PaginacaoViewModel<ConfiguracoesDePedido>> PaginacaoAsync(FilterModel<ConfiguracoesDePedido> filterModel)
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

    public void Update(ConfiguracoesDePedido entity)
    {
        throw new NotImplementedException();
    }
}
