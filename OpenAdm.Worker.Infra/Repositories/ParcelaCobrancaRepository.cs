using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Worker.Infra.Repositories;

public class ParcelaCobrancaRepository : IParcelaCobrancaRepository
{
    private readonly AppDbContext _appDbContext;

    public ParcelaCobrancaRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<ParcelaCobranca?> ObterPorIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ParcelaCobranca?> ObterPorIdAsNoTrackingAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(ParcelaCobranca parcelaCobranca)
    {
        await _appDbContext.ParcelasCobrancas.AddAsync(parcelaCobranca);
    }

    public void Update(ParcelaCobranca parcelaCobranca)
    {
        _appDbContext.ParcelasCobrancas.Update(parcelaCobranca);
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<int> ProximoNumeroParcela(Guid empresaOpenAdmId)
    {
        try
        {
            return await _appDbContext
                .ParcelasCobrancas
                .AsNoTracking()
                .Where(p => p.EmpresaOpenAdmId == empresaOpenAdmId)
                .MaxAsync(x => x.Numero) + 1;
        }
        catch (Exception)
        {
            return 1;
        }
    }

    public async Task<bool> TemCobrancaAsync(Guid empresaOpenAdmId, int mes, int ano)
    {
        return await _appDbContext
            .ParcelasCobrancas
            .AsNoTracking()
            .AnyAsync(x => x.EmpresaOpenAdmId == empresaOpenAdmId && x.MesCobranca == mes && x.AnoCobranca == ano);
    }

    public Task<PaginacaoViewModel<ParcelaCobranca>> PaginacaoAsync(FilterModel<ParcelaCobranca> filterModel)
    {
        throw new NotImplementedException();
    }
}