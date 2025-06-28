using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ConfiguracaoDePagamentoRepository : IConfiguracaoDePagamentoRepository
{
    private readonly AppDbContext _appDbContext;

    public ConfiguracaoDePagamentoRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(ConfiguracaoDePagamento configuracaoDePagamento)
    {
        await _appDbContext.AddAsync(configuracaoDePagamento);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<ConfiguracaoDePagamento?> GetAsync(Guid parceiroId)
    {
        return await _appDbContext
            .ConfiguracoesDePagamento
            .FirstOrDefaultAsync(x => x.ParceiroId == parceiroId);
    }

    public async Task UpdateAsync(ConfiguracaoDePagamento configuracaoDePagamento)
    {
        _appDbContext.Update(configuracaoDePagamento);
        await _appDbContext.SaveChangesAsync();
    }
}
