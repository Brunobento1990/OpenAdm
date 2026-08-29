namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IPedidoPublicoService
{
    Task<byte[]?> GerarPdfAsync(Guid empresaId, Guid idPublico);
}
