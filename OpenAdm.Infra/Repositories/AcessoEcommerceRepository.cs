using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class AcessoEcommerceRepository : IAcessoEcommerceRepository
{
    private readonly AppDbContext _appDbContext;

    public AcessoEcommerceRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(AcessoEcommerce acessoEcommerce)
    {
        await _appDbContext.AddAsync(acessoEcommerce);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task EditAsync(AcessoEcommerce acessoEcommerce)
    {
        await _appDbContext
            .AcessosEcommerce
            .Where(x => x.Id == acessoEcommerce.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Quantidade, acessoEcommerce.Quantidade));
    }

    public async Task<AcessoEcommerce?> ObterPorDataAsync(int ano, int mes, Guid parceiroId)
    {
        return await _appDbContext
            .AcessosEcommerce
            .FirstOrDefaultAsync(x => x.DataDeCriacao.Date.Year == ano && x.DataDeCriacao.Month == mes && x.ParceiroId == parceiroId);
    }
}
