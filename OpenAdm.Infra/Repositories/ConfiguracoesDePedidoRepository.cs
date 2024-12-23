using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracoesDePedidoRepository :GenericRepository<ConfiguracoesDePedido> , IConfiguracoesDePedidoRepository
{
    public ConfiguracoesDePedidoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }

    public async Task<ConfiguracoesDePedido?> GetConfiguracoesDePedidoAsync()
    {
        return await ParceiroContext
            .ConfiguracoesDePedidos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .FirstOrDefaultAsync(x => x.Ativo);
    }
}
