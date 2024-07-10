using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using Domain.Pkg.Pdfs.Services;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;
using System.Text;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class PedidoDownloadService : IPedidoDownloadService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IEnderecoEntregaPedidoRepository _enderecoEntregaPedidoRepository;
    public PedidoDownloadService(
        IPedidoRepository pedidoRepository,
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IEnderecoEntregaPedidoRepository enderecoEntregaPedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _enderecoEntregaPedidoRepository = enderecoEntregaPedidoRepository;
    }

    public async Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var configuracoesDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync();
        var logo = configuracoesDePedido?.Logo != null ? Encoding.UTF8.GetString(configuracoesDePedido.Logo) : null;
        var enderecoPedido = await _enderecoEntregaPedidoRepository.GetEnderecoEntregaPedidoByPedidoIdAsync(pedidoId);
        var pdf = PedidoPdfService.GeneratePdfAsync(pedido, enderecoPedido, logo);

        return pdf;
    }
}
