using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using System.Text;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class PedidoDownloadService : IPedidoDownloadService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    public PedidoDownloadService(
        IPedidoRepository pedidoRepository,
        IPdfPedidoService pdfPedidoService,
        IParceiroAutenticado parceiroAutenticado)
    {
        _pedidoRepository = pedidoRepository;
        _pdfPedidoService = pdfPedidoService;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new ExceptionApi("Não foi possível localizar o pedido!");

        var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();
        var logo = parceiro.Logo != null ? Encoding.UTF8.GetString(parceiro.Logo) : null;
        var pdf = _pdfPedidoService.GeneratePdfPedido(pedido, parceiro.NomeFantasia, logo);

        return pdf;
    }
}
