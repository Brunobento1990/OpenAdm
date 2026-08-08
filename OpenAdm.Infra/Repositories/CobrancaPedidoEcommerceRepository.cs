using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class CobrancaPedidoEcommerceRepository : GenericBaseRepository<CobrancaPedidoEcommerce>,
    ICobrancaPedidoEcommerceRepository
{
    public CobrancaPedidoEcommerceRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<CobrancaPedidoEcommerce?> GetByPedidoIdAsync(Guid pedidoId, Guid parceiroId)
    {
        return await AppDbContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PedidoId == pedidoId && x.ParceiroId == parceiroId);
    }

    public async Task AtualizarStatusAsync(Guid id, Guid parceiroId, StatusCobrancaPedidoEcommerceEnum status)
    {
        await AppDbContext
            .CobrancasPedidosEcommerce
            .Where(x => x.Id == id && x.ParceiroId == parceiroId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.Status, status)
                .SetProperty(y => y.DataDeAtualizacao, DateTime.UtcNow));
    }

    public async Task<decimal> TotalACobrarAposAsync(DateTime data, Guid parceiroId)
    {
        return await AppDbContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .Where(x => x.ParceiroId == parceiroId && x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar &&
                        x.Ativo &&
                        x.DataDeCriacao.Date >= data.Date)
            .SumAsync(x => x.Total);
    }

    public async Task<int> QuantidadeACobrarAsync(Guid parceiroId)
    {
        return await AppDbContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .Where(x => x.ParceiroId == parceiroId &&
                        x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar &&
                        x.Ativo)
            .CountAsync();
    }

    public async Task<decimal> TotalACobrarAsync(Guid parceiroId)
    {
        return await AppDbContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .Where(x => x.ParceiroId == parceiroId &&
                        x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar &&
                        x.Ativo)
            .SumAsync(x => x.Total);
    }

    public async Task<ICollection<CobrancaPedidoEcommerce>> CobrancasMaisAntigasAsync(Guid parceiroId)
    {
        return await AppDbContext
            .CobrancasPedidosEcommerce
            .AsNoTracking()
            .Where(x => x.ParceiroId == parceiroId &&
                        x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar &&
                        x.Ativo)
            .OrderBy(x => x.DataDeCriacao)
            .Take(3)
            .ToListAsync();
    }
}
