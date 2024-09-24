using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;
using System.Text;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class PedidoDownloadService : IPedidoDownloadService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IEnderecoEntregaPedidoRepository _enderecoEntregaPedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;
    public PedidoDownloadService(
        IPedidoRepository pedidoRepository,
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IEnderecoEntregaPedidoRepository enderecoEntregaPedidoRepository,
        IPdfPedidoService pdfPedidoService)
    {
        _pedidoRepository = pedidoRepository;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _enderecoEntregaPedidoRepository = enderecoEntregaPedidoRepository;
        _pdfPedidoService = pdfPedidoService;
    }

    public async Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var configuracoesDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync();
        var logo = configuracoesDePedido?.Logo != null ? Encoding.UTF8.GetString(configuracoesDePedido.Logo) : null;
        //var enderecoPedido = await _enderecoEntregaPedidoRepository.GetEnderecoEntregaPedidoByPedidoIdAsync(pedidoId);
        var pdf = _pdfPedidoService.GeneratePdfPedido(pedido, null, logo);

        return pdf;
    }
}
