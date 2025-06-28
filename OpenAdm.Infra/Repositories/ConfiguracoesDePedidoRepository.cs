using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracoesDePedidoRepository : GenericBaseRepository<ConfiguracoesDePedido> ,IConfiguracoesDePedidoRepository
{
    public ConfiguracoesDePedidoRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<ConfiguracoesDePedido?> GetConfiguracoesDePedidoAsync(Guid parceiroId)
    {
        return await AppDbContext
            .ConfiguracoesDePedidos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .FirstOrDefaultAsync(x => x.Ativo && x.ParceiroId == parceiroId);
    }
}
