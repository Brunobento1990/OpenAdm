using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracoesDePedidoRepository :GenericRepository<ConfiguracoesDePedido> , IConfiguracoesDePedidoRepository
{
    private readonly ParceiroContext _parceiroContext;

    public ConfiguracoesDePedidoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<ConfiguracoesDePedido?> GetConfiguracoesDePedidoAsync()
    {
        return await _parceiroContext
            .ConfiguracoesDePedidos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .FirstOrDefaultAsync(x => x.Ativo);
    }
}
