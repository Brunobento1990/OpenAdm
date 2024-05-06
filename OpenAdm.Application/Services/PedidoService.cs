using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;
using System.Reactive.Linq;

namespace OpenAdm.Application.Services;

public class PedidoService(
    IPedidoRepository pedidoRepository)
    : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<PedidoViewModel> GetAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(pedidoId)
            ?? throw new Exception($"Pedido não localizado: {pedidoId}");

        return new PedidoViewModel().ForModel(pedido);
    }

    public async Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(PaginacaoPedidoDto paginacaoPedidoDto)
    {
        var paginacao = await _pedidoRepository.GetPaginacaoPedidoAsync(paginacaoPedidoDto);

        return new PaginacaoViewModel<PedidoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new PedidoViewModel().ForModel(x)).ToList()
        };
    }

    public async Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido, Guid usuarioId)
    {
        var pedidos = await _pedidoRepository.GetPedidosByUsuarioIdAsync(usuarioId, statusPedido);
        return pedidos
            .Select(x => new PedidoViewModel().ForModel(x))
            .ToList();
    }
}
