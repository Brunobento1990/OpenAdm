﻿using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IPdfPedidoService
{
    byte[] GeneratePdfPedido(
        Pedido pedido,
        Parceiro parceiro);
    byte[] GeneratePdfPedidoRelatorio(GerarRelatorioPedidoDto relatorioPedidoDto, string nomeFantasia, IList<Pedido> pedido);
    byte[] ProducaoPedido(IList<ItemPedidoProducaoViewModel> itemPedidoProducaoViewModels, string nomeFantasia, string? logo, IList<string> pedidos);
}
