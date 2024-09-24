using Domain.Pkg.Entities;
using OpenAdm.Application.Dtos.Pedidos;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IPdfPedidoService
{
    byte[] GeneratePdfPedido(
        Pedido pedido,
        EnderecoEntregaPedido?
        enderecoEntregaPedido,
        string? logo);
    byte[] GeneratePdfPedidoRelatorio(GerarRelatorioPedidoDto relatorioPedidoDto);
}
