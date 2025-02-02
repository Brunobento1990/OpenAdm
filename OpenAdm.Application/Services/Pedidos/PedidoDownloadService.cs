using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using System.Text;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class PedidoDownloadService : IPedidoDownloadService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;
    public PedidoDownloadService(
        IPedidoRepository pedidoRepository,
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IPdfPedidoService pdfPedidoService)
    {
        _pedidoRepository = pedidoRepository;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _pdfPedidoService = pdfPedidoService;
    }

    public async Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new ExceptionApi("Não foi possível localizar o pedido!");

        var configuracoesDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync();
        var logo = configuracoesDePedido?.Logo != null ? Encoding.UTF8.GetString(configuracoesDePedido.Logo) : null;
        var pdf = _pdfPedidoService.GeneratePdfPedido(pedido, "Iscas lune", logo);

        return pdf;
    }
}
