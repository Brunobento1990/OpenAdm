using OpenAdm.Application.Services.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Pdf.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public class RelatorioPedidoPorPeriodoTest
{
    [Fact]
    public async Task GetListagemAsync_DeveRetornarTopTresProdutosPorQuantidadeEValorAgrupadosPorUsuario()
    {
        var usuario = UsuarioBuilder.Init().SemNome("Cliente").Build();
        var usuarioId = usuario.Id;
        var produtoA = ProdutoBuilder.Init().SemDescricao("Produto A").Build();
        var produtoB = ProdutoBuilder.Init().SemDescricao("Produto B").Build();
        var produtoC = ProdutoBuilder.Init().SemDescricao("Produto C").Build();
        var produtoD = ProdutoBuilder.Init().SemDescricao("Produto D").Build();

        var pedidos = new List<Pedido>
        {
            PedidoBuilder.Init().ComStatusPedido(StatusPedido.Entregue).ComUsuario(usuario)
                .ComProdutos((produtoA, 2, 100), (produtoB, 8, 5), (produtoD, 1, 1)).Build(),
            PedidoBuilder.Init().ComStatusPedido(StatusPedido.Entregue).ComUsuario(usuario)
                .ComProdutos((produtoA, 3, 100), (produtoC, 4, 20)).Build()
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

}
