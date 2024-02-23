using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using Domain.Pkg.Model;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Domain.Test;

public class PedidoTest
{
    [Fact]
    public void NaoDeveCriarPedidoSemUsuario()
    {
        Assert.Throws<ExceptionApi>(() => PedidoBuilder.Init().SemUsuario(Guid.Empty).Build());
        Assert.Throws<ExceptionApi>(() => PedidoBuilder.Init().SemUsuario(null).Build());
    }

    [Fact]
    public void DeveProcessarItensPedidoCorretamente()
    {
        var pedidoPorPesoModel = new PedidoPorPesoModel(Guid.NewGuid(), Guid.NewGuid(), 1);
        var pedidoPorTamanhoModel = new PedidoPorTamanhoModel(Guid.NewGuid(), Guid.NewGuid(), 1);
        var tabelaDePreco = TabelaDePrecoBuilder.Init().Build();

        tabelaDePreco.ItensTabelaDePreco.AddRange(new List<ItensTabelaDePreco>()
        {
            new (
                Guid.NewGuid(),
                tabelaDePreco.DataDeCriacao,
                tabelaDePreco.DataDeAtualizacao,
                0,
                pedidoPorPesoModel.ProdutoId,
                2,
                tabelaDePreco.Id,
                null,
                pedidoPorPesoModel.PesoId),
            new (
                Guid.NewGuid(),
                tabelaDePreco.DataDeCriacao,
                tabelaDePreco.DataDeAtualizacao,
                0,
                pedidoPorTamanhoModel.ProdutoId,
                2,
                tabelaDePreco.Id,
                pedidoPorTamanhoModel.TamanhoId,
                null)
        });

        var pedido = PedidoBuilder.Init().Build();

        pedido.ProcessarItensPedido(
            new List<PedidoPorPesoModel>() { pedidoPorPesoModel }, 
            new List<PedidoPorTamanhoModel>() { pedidoPorTamanhoModel }, 
            tabelaDePreco);

        var item1 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == pedidoPorPesoModel.ProdutoId && x.PesoId == pedidoPorPesoModel.PesoId);
        
        var item2 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == pedidoPorTamanhoModel.ProdutoId && x.TamanhoId == pedidoPorTamanhoModel.TamanhoId);


        var vlrUnitario1 = tabelaDePreco
            .ItensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == pedidoPorPesoModel.ProdutoId && x.PesoId == pedidoPorPesoModel.PesoId)?.ValorUnitario ?? 0;

        var vlrUnitario2 = tabelaDePreco
            .ItensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == pedidoPorPesoModel.ProdutoId && x.PesoId == pedidoPorPesoModel.PesoId)?.ValorUnitario ?? 0;


        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.Equal(pedidoPorPesoModel.PesoId, item1.PesoId);
        Assert.Equal(pedidoPorTamanhoModel.TamanhoId, item2.TamanhoId);
        Assert.Equal(vlrUnitario1, item1.ValorUnitario);
        Assert.Equal(vlrUnitario2, item2.ValorUnitario);
        Assert.Equal(pedidoPorPesoModel.Quantidade, item1.Quantidade);
        Assert.Equal(pedidoPorTamanhoModel.Quantidade, item2.Quantidade);

    }

    [Fact]
    public void DeveProcessarItensPedidoCorretamenteSemPesoESemTamanho()
    {
        var pedidoPorPesoModel = new PedidoPorPesoModel(Guid.NewGuid(), Guid.NewGuid(), 1);
        var pedidoPorTamanhoModel = new PedidoPorTamanhoModel(Guid.NewGuid(), Guid.NewGuid(), 1);
        var tabelaDePreco = TabelaDePrecoBuilder.Init().Build();

        tabelaDePreco.ItensTabelaDePreco.AddRange(new List<ItensTabelaDePreco>()
        {
            new (
                Guid.NewGuid(),
                tabelaDePreco.DataDeCriacao,
                tabelaDePreco.DataDeAtualizacao,
                0,
                pedidoPorPesoModel.ProdutoId,
                2,
                tabelaDePreco.Id,
                null,
                null),
            new (
                Guid.NewGuid(),
                tabelaDePreco.DataDeCriacao,
                tabelaDePreco.DataDeAtualizacao,
                0,
                pedidoPorTamanhoModel.ProdutoId,
                2,
                tabelaDePreco.Id,
                null,
                null)
        });

        var pedido = PedidoBuilder.Init().Build();

        pedido.ProcessarItensPedido(
            new List<PedidoPorPesoModel>() { pedidoPorPesoModel },
            new List<PedidoPorTamanhoModel>() { pedidoPorTamanhoModel },
            tabelaDePreco);

        var item1 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == pedidoPorPesoModel.ProdutoId);

        var item2 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == pedidoPorTamanhoModel.ProdutoId);


        var vlrUnitario1 = tabelaDePreco
            .ItensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == pedidoPorPesoModel.ProdutoId)?.ValorUnitario ?? 0;

        var vlrUnitario2 = tabelaDePreco
            .ItensTabelaDePreco
            .FirstOrDefault(x => x.ProdutoId == pedidoPorPesoModel.ProdutoId)?.ValorUnitario ?? 0;


        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.Equal(vlrUnitario1, item1.ValorUnitario);
        Assert.Equal(vlrUnitario2, item2.ValorUnitario);
        Assert.Equal(pedidoPorPesoModel.Quantidade, item1.Quantidade);
        Assert.Equal(pedidoPorTamanhoModel.Quantidade, item2.Quantidade);

    }
}
