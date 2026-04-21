using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Worker.Infra.Repositories;

public class ParcelaCobrancaRepository : IParcelaCobrancaRepository
{
    private readonly AppDbContext _appDbContext;

    public ParcelaCobrancaRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
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
}