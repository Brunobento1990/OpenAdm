using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ItensPedidoRepository : GenericRepository<ItemPedido>, IItensPedidoRepository
{
    public ItensPedidoRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
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

    public async Task<IList<ItemPedido>> GetItensPedidoByProducaoAsync(IList<Guid> pedidosIds, IList<Guid> produtosIds, IList<Guid> pesosIds, IList<Guid> tamanhosIds)
    {
        var itens = _parceiroContext
            .ItensPedidos
            .AsNoTracking()
            .Include(x => x.Pedido)
                .ThenInclude(x => x.Usuario)
            .Include(x => x.Produto)
                .ThenInclude(x => x.Categoria)
            .Include(x => x.Peso)
            .Include(x => x.Tamanho)
            .Where(x => x.Pedido.StatusPedido == Domain.Enuns.StatusPedido.Aberto);

        if (pedidosIds.Count > 0)
        {
            itens = itens.Where(x => pedidosIds.Contains(x.PedidoId));
        }

        if (produtosIds.Count > 0)
        {
            itens = itens.Where(x => produtosIds.Contains(x.ProdutoId));
        }

        if (tamanhosIds.Count > 0)
        {
            itens = itens.Where(x => tamanhosIds.Contains(x.TamanhoId!.Value));
        }

        if (pesosIds.Count > 0)
        {
            itens = itens.Where(x => pesosIds.Contains(x.PesoId!.Value));
        }

        return await itens.ToListAsync();
    }
}
