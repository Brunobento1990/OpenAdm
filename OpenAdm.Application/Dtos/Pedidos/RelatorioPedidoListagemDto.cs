namespace OpenAdm.Application.Dtos.Pedidos;

public sealed record RelatorioPedidoListagemDto(
    IEnumerable<RelatorioPedidoItemDto> Values,
    RelatorioPedidoTotaisDto Totais,
    IEnumerable<RelatorioPedidoTotaisUsuarioDto> TotaisPorUsuario);

public sealed record RelatorioPedidoItemDto(
    Guid PedidoId,
    long Numero,
    Guid UsuarioId,
    string Usuario,
    decimal QuantidadeItens,
    decimal ValorTotal,
    DateTime DataDeCriacao);

public sealed record RelatorioPedidoTotaisDto(
    int QuantidadePedidos,
    decimal QuantidadeItens,
    decimal ValorTotal);

public sealed record RelatorioPedidoTotaisUsuarioDto(
    Guid UsuarioId,
    string Usuario,
    int QuantidadePedidos,
    decimal QuantidadeItens,
    decimal ValorTotal,
    IEnumerable<RelatorioPedidoProdutoDto> TopProdutosPorQuantidade,
    IEnumerable<RelatorioPedidoProdutoDto> TopProdutosPorValor);

public sealed record RelatorioPedidoProdutoDto(
    Guid ProdutoId,
    string Produto,
    decimal Quantidade,
    decimal ValorTotal);
