using Microsoft.EntityFrameworkCore;
using Domain.Pkg.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ItensPedidoRepository : GenericRepository<ItensPedido>, IItensPedidoRepository
{
    private readonly ParceiroContext _parceiroContext;

    public ItensPedidoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<ItensPedido?> GetItemPedidoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .ItensPedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<ItensPedido>> GetItensPedidoByPedidoIdAsync(Guid pedidoId)
    {
        var itens = await _parceiroContext
            .ItensPedidos
            .AsNoTracking()
            .Include(x => x.Produto)
            .Include(x => x.Tamanho)
            .Include(x => x.Peso)
            .Where(x => x.PedidoId == pedidoId)
            .ToListAsync();

        foreach (var item in itens)
        {
            item.Produto.ItensPedido = new();

            if (item.Tamanho != null)
                item.Tamanho.ItensPedido = new();

            if (item.Peso != null)
                item.Peso.ItensPedido = new();

            item.Pedido = null!;
        }

        return itens;
    }
}
