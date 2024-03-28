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

    public PedidoDownloadService(
        IPedidoRepository pedidoRepository, 
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
    }

    public async Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var configuracoesDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync();
        var logo = configuracoesDePedido?.Logo != null ? Encoding.UTF8.GetString(configuracoesDePedido.Logo) : null;

        var pdf = PedidoPdfService.GeneratePdfAsync(pedido, logo);

        return pdf;
    }
}
