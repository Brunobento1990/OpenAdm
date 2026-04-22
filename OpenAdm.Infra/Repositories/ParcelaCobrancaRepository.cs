using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Extensions.IQueryable;

namespace OpenAdm.Infra.Repositories;

public class ParcelaCobrancaRepository : IParcelaCobrancaRepository
{
    private readonly AppDbContext _appDbContext;

    public ParcelaCobrancaRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ParcelaCobranca?> ObterPorIdAsync(Guid id)
    {
        return await _appDbContext.ParcelasCobrancas.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ParcelaCobranca?> ObterPorIdAsNoTrackingAsync(Guid id)
    {
        return await _appDbContext.ParcelasCobrancas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task AddAsync(ParcelaCobranca parcelaCobranca)
    {
        throw new NotImplementedException();
    }

    public void Update(ParcelaCobranca parcelaCobranca)
    {
        _appDbContext.ParcelasCobrancas.Update(parcelaCobranca);
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public Task<int> ProximoNumeroParcela(Guid empresaOpenAdmId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TemCobrancaAsync(Guid empresaOpenAdmId, int mes, int ano)
    {
        throw new NotImplementedException();
    }

    public async Task<PaginacaoViewModel<ParcelaCobranca>> PaginacaoAsync(FilterModel<ParcelaCobranca> filterModel)
    {
        var query = _appDbContext
            .ParcelasCobrancas
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.EmpresaOpenAdmId == filterModel.ParceiroId);

        var (TotalPaginas, Values) = await query
            .CustomFilterAsync(filterModel);

        var totalDeRegistros =
            await _appDbContext.ParcelasCobrancas.Where(x => x.EmpresaOpenAdmId == filterModel.ParceiroId).CountAsync();

        return new()
        {
            TotalPaginas = TotalPaginas,
            Values = Values,
            TotalDeRegistros = totalDeRegistros
        };
    }

    public async Task UpdateIdExternoAsync(Guid id, string idExterno)
    {
        await _appDbContext
            .ParcelasCobrancas
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.IdExterno, idExterno));
    }

    public async Task<ParcelaCobranca?> ObterPorIdExternoAsync(string idExterno)
    {
        return await _appDbContext
            .ParcelasCobrancas
            .FirstOrDefaultAsync(x => x.IdExterno == idExterno);
    }
}