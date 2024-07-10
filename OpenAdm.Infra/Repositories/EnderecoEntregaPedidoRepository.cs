using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class EnderecoEntregaPedidoRepository : IEnderecoEntregaPedidoRepository
{
    private readonly ParceiroContext _parceiroContext;

    public EnderecoEntregaPedidoRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<EnderecoEntregaPedido?> GetEnderecoEntregaPedidoByPedidoIdAsync(Guid pedidoId)
    {
        return await _parceiroContext
            .EnderecoEntregaPedido
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PedidoId == pedidoId);
    }
}
