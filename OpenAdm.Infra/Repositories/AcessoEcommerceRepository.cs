using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class AcessoEcommerceRepository : IAcessoEcommerceRepository
{
    private readonly ParceiroContext _parceiroContext;

    public AcessoEcommerceRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task AddAsync(AcessoEcommerce acessoEcommerce)
    {
        await _parceiroContext.AddAsync(acessoEcommerce);
        await _parceiroContext.SaveChangesAsync();
    }

    public async Task EditAsync(AcessoEcommerce acessoEcommerce)
    {
        await _parceiroContext
            .AcessosEcommerce
            .Where(x => x.Id == acessoEcommerce.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Quantidade, acessoEcommerce.Quantidade));
    }

    public async Task<AcessoEcommerce?> ObterPorDataAsync(DateTime data, DateTime dataFinal)
    {
        return await _parceiroContext
            .AcessosEcommerce
            .FirstOrDefaultAsync(x => x.DataDeCriacao.Date >= data.Date || x.DataDeCriacao.Date <= dataFinal.Date);
    }
}
