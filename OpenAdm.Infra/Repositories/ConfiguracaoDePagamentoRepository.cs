using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ConfiguracaoDePagamentoRepository : IConfiguracaoDePagamentoRepository
{
    private readonly ParceiroContext _parceiroContext;

    public ConfiguracaoDePagamentoRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task AddAsync(ConfiguracaoDePagamento configuracaoDePagamento)
    {
        await _parceiroContext.AddAsync(configuracaoDePagamento);
        await _parceiroContext.SaveChangesAsync();
    }

    public async Task<ConfiguracaoDePagamento?> GetAsync()
    {
        return await _parceiroContext
            .ConfiguracoesDePagamento
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(ConfiguracaoDePagamento configuracaoDePagamento)
    {
        _parceiroContext.Attach(configuracaoDePagamento);
        _parceiroContext.Update(configuracaoDePagamento);
        await _parceiroContext.SaveChangesAsync();
    }
}
