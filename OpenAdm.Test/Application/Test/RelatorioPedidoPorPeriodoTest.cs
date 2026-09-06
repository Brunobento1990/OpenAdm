using OpenAdm.Application.Services.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Pdf.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class RelatorioPedidoPorPeriodoTest
{
    [Fact]
    public async Task GetListagemAsync_DeveRetornarTopTresProdutosPorQuantidadeEValorAgrupadosPorUsuario()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = CriarUsuario(usuarioId, "Cliente");
        var produtoA = CriarProduto("Produto A");
        var produtoB = CriarProduto("Produto B");
        var produtoC = CriarProduto("Produto C");
        var produtoD = CriarProduto("Produto D");

        var pedidos = new List<Pedido>
        {
            CriarPedido(usuario, (produtoA, 2, 100), (produtoB, 8, 5), (produtoD, 1, 1)),
            CriarPedido(usuario, (produtoA, 3, 100), (produtoC, 4, 20))
        };

        var pedidoRepository = new Mock<IPedidoRepository>();
        pedidoRepository
            .Setup(x => x.GetPedidosByRelatorioPorPeriodoAsync(It.IsAny<RelatorioPedidoDto>()))
            .ReturnsAsync(pedidos);

        var service = new RelatorioPedidoPorPeriodo(
            pedidoRepository.Object,
            Mock.Of<IPdfPedidoService>(),
            Mock.Of<IParceiroAutenticado>());

        var resultado = await service.GetListagemAsync(new RelatorioPedidoDto
        {
            UsuarioId = usuarioId
        });

        var totaisUsuario = Assert.Single(resultado.TotaisPorUsuario);

        Assert.Equal(
            [produtoB.Id, produtoA.Id, produtoC.Id],
            totaisUsuario.TopProdutosPorQuantidade.Select(x => x.ProdutoId));
        Assert.Equal(
            [produtoA.Id, produtoC.Id, produtoB.Id],
            totaisUsuario.TopProdutosPorValor.Select(x => x.ProdutoId));

        var produtoAAgrupado = Assert.Single(
            totaisUsuario.TopProdutosPorValor, x => x.ProdutoId == produtoA.Id);
        Assert.Equal(5, produtoAAgrupado.Quantidade);
        Assert.Equal(500, produtoAAgrupado.ValorTotal);
        Assert.DoesNotContain(
            totaisUsuario.TopProdutosPorQuantidade,
            x => x.ProdutoId == produtoD.Id);
    }

    private static Pedido CriarPedido(
        Usuario usuario,
        params (Produto produto, decimal quantidade, decimal valorUnitario)[] produtos)
    {
        var pedido = new Pedido(
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow,
            1,
            StatusPedido.Entregue,
            usuario.Id,
            null)
        {
            Usuario = usuario
        };

        pedido.ItensPedido = produtos
            .Select(x => new ItemPedido(
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                1,
                null,
                null,
                x.produto.Id,
                pedido.Id,
                x.valorUnitario,
                x.quantidade)
            {
                Produto = x.produto
            })
            .ToList();

        return pedido;
    }

    private static Usuario CriarUsuario(Guid id, string nome) => new(
        id,
        DateTime.UtcNow,
        DateTime.UtcNow,
        1,
        "cliente@teste.com",
        "senha",
        nome,
        null,
        null,
        null,
        true,
        null,
        null,
        null);

    private static Produto CriarProduto(string descricao) => new(
        Guid.NewGuid(),
        DateTime.UtcNow,
        DateTime.UtcNow,
        1,
        descricao,
        null,
        Guid.NewGuid(),
        null,
        null,
        null,
        false,
        false,
        true);
}
