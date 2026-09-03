namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IPedidoDownloadService
{
    Task<(byte[] Pdf, long NumeroPedido)> DownloadPedidoPdfAsync(Guid pedidoId);
}
