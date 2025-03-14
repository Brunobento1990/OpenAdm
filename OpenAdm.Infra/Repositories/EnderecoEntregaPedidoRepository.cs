﻿using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
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

    public async Task AddAsync(EnderecoEntregaPedido enderecoEntregaPedido)
    {
        await _parceiroContext.AddAsync(enderecoEntregaPedido);
        await _parceiroContext.SaveChangesAsync();
    }

    public async Task<EnderecoEntregaPedido?> GetByPedidoIdAsync(Guid pedidoId)
    {
        return await _parceiroContext
            .EnderecosEntregaPedido
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PedidoId == pedidoId);
    }
}
