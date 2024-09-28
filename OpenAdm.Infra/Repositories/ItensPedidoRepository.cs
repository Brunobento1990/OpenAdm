using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ItensPedidoRepository : GenericRepository<ItemPedido>, IItensPedidoRepository
{
    private readonly ParceiroContext _parceiroContext;

    public ItensPedidoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<ItemPedido?> GetItemPedidoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .ItensPedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<ItemPedido>> GetItensPedidoByPedidoIdAsync(Guid pedidoId)
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
            item.Produto.ItensPedido = [];

            if (item.Tamanho != null)
                item.Tamanho.ItensPedido = [];

            if (item.Peso != null)
                item.Peso.ItensPedido = [];

            item.Pedido = null!;
        }

        return itens;
    }
}
