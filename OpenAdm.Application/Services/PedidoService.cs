﻿using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.EnderecosEntregaPedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;
using System.Reactive.Linq;

namespace OpenAdm.Application.Services;

public class PedidoService(
    IPedidoRepository pedidoRepository,
    IEnderecoEntregaPedidoRepository enderecoEntregaPedidoRepository)
    : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;
    private readonly IEnderecoEntregaPedidoRepository _enderecoEntregaPedidoRepository = enderecoEntregaPedidoRepository;

    public async Task<PedidoViewModel> GetAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new Exception($"Pedido não localizado: {pedidoId}");
        var enderecoEntrega = await _enderecoEntregaPedidoRepository.GetEnderecoEntregaPedidoByPedidoIdAsync(pedidoId);

        var pedidoViewModel = new PedidoViewModel().ForModel(pedido);
        pedidoViewModel.EnderecoEntrega = EnderecoEntregaPedidoViewModel.ToEntity(enderecoEntrega);

        return pedidoViewModel;
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
