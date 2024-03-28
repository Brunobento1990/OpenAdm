namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IPedidoDownloadService
{
    Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId);
}
