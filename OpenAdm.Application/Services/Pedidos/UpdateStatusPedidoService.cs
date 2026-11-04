using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public class UpdateStatusPedidoService : IUpdateStatusPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IMovimentacaoDeProdutosService _movimentacaoDeProdutosService;
    private readonly IEstoqueService _estoqueService;

    public UpdateStatusPedidoService(
        IPedidoRepository pedidoRepository,
        IMovimentacaoDeProdutosService movimentacaoDeProdutosService,
        IEstoqueService estoqueService)
    {
        _pedidoRepository = pedidoRepository;
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
        _estoqueService = estoqueService;
    }

    public async Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(updateStatusPedidoDto.PedidoId)
                     ?? throw new ExceptionApi("Não foi possível localizar o pedido");
        var statusAnterior = pedido.StatusPedido;
        pedido.UpdateStatus(updateStatusPedidoDto.StatusPedido);

        if (statusAnterior != pedido.StatusPedido)
        {
            pedido.Historicos.Add(PedidoHistorico.Novo(
                pedido.Id,
                $"Status do pedido alterado de {statusAnterior} para {pedido.StatusPedido}."));
        }

        _pedidoRepository.Update(pedido);

        if (pedido.StatusPedido == StatusPedido.Entregue)
        {
            await _movimentacaoDeProdutosService.MovimentarItensPedidoAsync(pedido.ItensPedido);
            await _estoqueService.MovimentacaoDePedidoEntregueAsync(pedido.ItensPedido);
        }

        await _pedidoRepository.SaveChangesAsync();

        return new PedidoViewModel().ForModel(pedido);
    }
}
