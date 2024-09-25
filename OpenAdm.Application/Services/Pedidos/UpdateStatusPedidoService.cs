using Domain.Pkg.Enum;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public class UpdateStatusPedidoService : IUpdateStatusPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarPedidoService _processarPedidoService;

    public UpdateStatusPedidoService(
        IPedidoRepository pedidoRepository, 
        IProcessarPedidoService processarPedidoService)
    {
        _pedidoRepository = pedidoRepository;
        _processarPedidoService = processarPedidoService;
    }

    public async Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(updateStatusPedidoDto.PedidoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);
        pedido.UpdateStatus(updateStatusPedidoDto.StatusPedido);

        await _pedidoRepository.UpdateAsync(pedido);

        if (pedido.StatusPedido == StatusPedido.Entregue)
        {
            await _processarPedidoService.ProcessarProdutosMaisVendidosAsync(pedido);
        }

        return new PedidoViewModel().ForModel(pedido);
    }
}
