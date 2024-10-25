using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public sealed class PedidoCached : IPedidoRepository
{
    private readonly PedidoRepository _pedidoRepository;
    private readonly ICachedService<Pedido> _cachedService;
    private const string KEY = "pedido-{0}";

    public PedidoCached(
        PedidoRepository pedidoRepository,
        ICachedService<Pedido> cachedService)
    {
        _pedidoRepository = pedidoRepository;
        _cachedService = cachedService;
    }

    public Task<Pedido> AddAsync(Pedido entity)
        => _pedidoRepository.AddAsync(entity);

    public async Task<bool> DeleteAsync(Pedido entity)
    {
        var key = string.Format(KEY, entity.Id);
        await _cachedService.RemoveCachedAsync(key);
        return await _pedidoRepository.DeleteAsync(entity);
    }

    public Task<int> GetCountPedidosEmAbertoAsync()
        => _pedidoRepository.GetCountPedidosEmAbertoAsync();

    public Task<PaginacaoViewModel<Pedido>> GetPaginacaoPedidoAsync(FilterModel<Pedido> filterModel)
        => _pedidoRepository.GetPaginacaoPedidoAsync(filterModel);

    public async Task<Pedido?> GetPedidoByIdAsync(Guid id)
    {
        var key = string.Format(KEY, id);
        var pedido = await _cachedService.GetItemAsync(key);

        if (pedido == null)
        {
            pedido = await _pedidoRepository.GetPedidoByIdAsync(id);

            if (pedido != null)
            {
                await _cachedService.SetItemAsync(key, pedido);
            }
        }

        return pedido;
    }

    public async Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id)
    {
        var key = string.Format(KEY, id);
        var pedido = await _cachedService.GetItemAsync(key);

        if (pedido == null)
        {
            pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(id);

            if (pedido != null)
            {
                await _cachedService.SetItemAsync(key, pedido);
            }
        }

        return pedido;
    }

    public Task<IList<Pedido>> GetPedidosByRelatorioPorPeriodoAsync(RelatorioPedidoDto relatorioPedidoDto)
        => _pedidoRepository.GetPedidosByRelatorioPorPeriodoAsync(relatorioPedidoDto);

    public async Task<IList<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido)
    {
        var key = $"pedido-{usuarioId}-{statusPedido}";
        var pedidos = await _cachedService.GetListItemAsync(key);

        if (pedidos == null)
        {
            pedidos = await _pedidoRepository.GetPedidosByUsuarioIdAsync(usuarioId, statusPedido);

            if (pedidos.Count > 0)
            {
                await _cachedService.SetListItemAsync(key, pedidos);
            }
        }

        return pedidos;
    }

    public async Task<IList<Pedido>> GetPedidosEmAbertoAsync()
    {
        var key = "pedidos-aberto";
        var pedidos = await _cachedService.GetListItemAsync(key);

        if (pedidos == null)
        {
            pedidos = await _pedidoRepository.GetPedidosEmAbertoAsync();
            if (pedidos.Count > 0)
            {
                await _cachedService.SetListItemAsync(key, pedidos);
            }
        }

        return pedidos;
    }

    public Task<int> GetQuantidadeDePedidoPorUsuarioAsync(Guid usuarioId)
        => _pedidoRepository.GetQuantidadeDePedidoPorUsuarioAsync(usuarioId);

    public Task<int> GetQuantidadePorStatusUsuarioAsync(Guid usuarioId, StatusPedido statusPedido)
        => _pedidoRepository.GetQuantidadePorStatusUsuarioAsync(usuarioId, statusPedido);

    public Task<decimal> GetTotalPedidoPorUsuarioAsync(Guid usuarioId)
        => _pedidoRepository.GetTotalPedidoPorUsuarioAsync(usuarioId);

    public Task<PaginacaoViewModel<Pedido>> PaginacaoAsync(FilterModel<Pedido> filterModel)
        => _pedidoRepository.PaginacaoAsync(filterModel);

    public async Task<Pedido> UpdateAsync(Pedido entity)
    {
        var key = string.Format(KEY, entity.Id);
        await _cachedService.RemoveCachedAsync(key);
        return await _pedidoRepository.UpdateAsync(entity);
    }
}
