using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IPdfPedidoService
{
    byte[] GeneratePdfPedido(
        Pedido pedido,
        string empresa,
        string? logo);
    byte[] GeneratePdfPedidoRelatorio(GerarRelatorioPedidoDto relatorioPedidoDto, string nomeFantasia);
}
