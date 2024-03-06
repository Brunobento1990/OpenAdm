using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using Domain.Pkg.Model;
using OpenAdm.Application.Services;
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
        var pedidoPorPesoModel = new PedidoPorPesoModel(Guid.NewGuid(), Guid.NewGuid(), 1, 8);
        var pedidoPorTamanhoModel = new PedidoPorTamanhoModel(Guid.NewGuid(), Guid.NewGuid(), 1, 9);

        var pedido = PedidoBuilder.Init().Build();

        pedido.ProcessarItensPedido(
            new List<PedidoPorPesoModel>() { pedidoPorPesoModel }, 
            new List<PedidoPorTamanhoModel>() { pedidoPorTamanhoModel });

        var item1 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == pedidoPorPesoModel.ProdutoId && x.PesoId == pedidoPorPesoModel.PesoId);
        
        var item2 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == pedidoPorTamanhoModel.ProdutoId && x.TamanhoId == pedidoPorTamanhoModel.TamanhoId);

        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.Equal(pedidoPorPesoModel.PesoId, item1.PesoId);
        Assert.Equal(pedidoPorTamanhoModel.TamanhoId, item2.TamanhoId);
        Assert.Equal(pedidoPorPesoModel.ValorUnitario, item1.ValorUnitario);
        Assert.Equal(pedidoPorTamanhoModel.ValorUnitario, item2.ValorUnitario);
        Assert.Equal(pedidoPorPesoModel.Quantidade, item1.Quantidade);
        Assert.Equal(pedidoPorTamanhoModel.Quantidade, item2.Quantidade);

    }

    [Fact]
    public void DeveProcessarItensPedidoCorretamenteSemPesoESemTamanho()
    {
        var pedidoPorPesoModel = new PedidoPorPesoModel(Guid.NewGuid(), Guid.NewGuid(), 1, 8);
        var pedidoPorTamanhoModel = new PedidoPorTamanhoModel(Guid.NewGuid(), Guid.NewGuid(), 1, 9);

        var pedido = PedidoBuilder.Init().Build();

        pedido.ProcessarItensPedido(
            new List<PedidoPorPesoModel>() { pedidoPorPesoModel },
            new List<PedidoPorTamanhoModel>() { pedidoPorTamanhoModel });

        var item1 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == pedidoPorPesoModel.ProdutoId);

        var item2 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == pedidoPorTamanhoModel.ProdutoId);


        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.Equal(pedidoPorPesoModel.ValorUnitario, item1.ValorUnitario);
        Assert.Equal(pedidoPorTamanhoModel.ValorUnitario, item2.ValorUnitario);
        Assert.Equal(pedidoPorPesoModel.Quantidade, item1.Quantidade);
        Assert.Equal(pedidoPorTamanhoModel.Quantidade, item2.Quantidade);

    }
}
