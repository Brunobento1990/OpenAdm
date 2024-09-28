using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public class UpdateStatusPedidoService : IUpdateStatusPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutosMaisVendidosService _produtosMaisVendidosService;
    private readonly ITopUsuarioService _topUsuarioService;
    private readonly IMovimentacaoDeProdutosService _movimentacaoDeProdutosService;

    public UpdateStatusPedidoService(
        IPedidoRepository pedidoRepository,
        IProdutosMaisVendidosService produtosMaisVendidosService,
        ITopUsuarioService topUsuarioService,
        IMovimentacaoDeProdutosService movimentacaoDeProdutosService)
    {
        _pedidoRepository = pedidoRepository;
        _produtosMaisVendidosService = produtosMaisVendidosService;
        _topUsuarioService = topUsuarioService;
        _movimentacaoDeProdutosService = movimentacaoDeProdutosService;
    }

    public async Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(updateStatusPedidoDto.PedidoId)
            ?? throw new ExceptionApi("Não foi possível localizar o pedido");
        pedido.UpdateStatus(updateStatusPedidoDto.StatusPedido);

        await _pedidoRepository.UpdateAsync(pedido);

        if (pedido.StatusPedido == StatusPedido.Entregue)
        {
            await _produtosMaisVendidosService.ProcessarAsync(pedido);
            await _topUsuarioService.AddOrUpdateTopUsuarioAsync(pedido);
            await _movimentacaoDeProdutosService.MovimentarItensPedidoAsync(pedido.ItensPedido);
        }

        return new PedidoViewModel().ForModel(pedido);
    }
}
