using OpenAdm.Domain.Entities;
using OpenAdm.Pdf.DTOs;

namespace OpenAdm.Pdf.Interfaces;

public interface IPdfPedidoService
{
    byte[] GeneratePdfPedido(
    Pedido pedido,
    Parceiro parceiro);
    byte[] GeneratePdfPedidoRelatorio(GerarRelatorioPedidoDTO relatorioPedidoDto, string nomeFantasia, IList<Pedido> pedido);
    byte[] ProducaoPedido(IList<ItemPedidoProducaoDTO> itemPedidoProducaoViewModels, string nomeFantasia, string? logo, IList<string> pedidos);
}
