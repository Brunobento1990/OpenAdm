using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class CobrancaPedidoEcommerceRepository(ParceiroContext parceiroContext) : ICobrancaPedidoEcommerceRepository
{
    public async Task AddAsync(CobrancaPedidoEcommerce cobranca)
    {
        await parceiroContext.CobrancasPedidosEcommerce.AddAsync(cobranca);
    }

    public async Task SaveChangesAsync() => await parceiroContext.SaveChangesAsync();

    public async Task<CobrancaPedidoEcommerce?> GetByPedidoIdAsync(Guid pedidoId)
    {
        return await parceiroContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PedidoId == pedidoId);
    }

    public async Task AtualizarStatusAsync(Guid id, StatusCobrancaPedidoEcommerceEnum status)
    {
        await parceiroContext
            .CobrancasPedidosEcommerce
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.Status, status)
                .SetProperty(y => y.DataDeAtualizacao, DateTime.UtcNow));
    }

    public async Task<decimal> TotalACobrarAposAsync(DateTime data)
    {
        return await parceiroContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .Where(x => x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar &&
                        x.Ativo &&
                        x.DataDeCriacao >= data)
            .SumAsync(x => x.Total);
    }

    public async Task<int> QuantidadeACobrarAsync()
    {
        return await parceiroContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .Where(x => x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar &&
                        x.Ativo)
            .CountAsync();
    }

    public async Task<decimal> TotalACobrarAsync()
    {
        return await parceiroContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .Where(x => x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar &&
                        x.Ativo)
            .SumAsync(x => x.Total);
    }

    public async Task<ICollection<CobrancaPedidoEcommerce>> CobrancasMaisAntigasAsync()
    {
        return await parceiroContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .Where(x => x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar &&
                        x.Ativo)
            .OrderBy(x => x.DataDeCriacao)
            .Take(3)
            .ToListAsync();
    }
}
