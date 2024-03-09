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
        var primeiroProduto = new ItensPedidoModel()
        {
            ProdutoId = Guid.NewGuid(),
            Quantidade = 5,
            ValorUnitario = 187,
            PesoId = Guid.NewGuid()
        };
        var segundoProduto = new ItensPedidoModel()
        {
            ProdutoId = Guid.NewGuid(),
            Quantidade = 2,
            ValorUnitario = 947,
            TamanhoId = Guid.NewGuid()
        };

        var itensPedidoModel = new List<ItensPedidoModel>()
        {
           primeiroProduto,
           segundoProduto
        };

        var pedido = PedidoBuilder.Init().Build();

        pedido.ProcessarItensPedido(itensPedidoModel);

        var item1 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == primeiroProduto.ProdutoId && x.PesoId == primeiroProduto.PesoId);

        var item2 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == segundoProduto.ProdutoId && x.TamanhoId == segundoProduto.TamanhoId);

        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.Equal(primeiroProduto.PesoId, item1.PesoId);
        Assert.Equal(segundoProduto.TamanhoId, item2.TamanhoId);
        Assert.Equal(primeiroProduto.ValorUnitario, item1.ValorUnitario);
        Assert.Equal(segundoProduto.ValorUnitario, item2.ValorUnitario);
        Assert.Equal(primeiroProduto.Quantidade, item1.Quantidade);
        Assert.Equal(segundoProduto.Quantidade, item2.Quantidade);

    }

    [Fact]
    public void DeveProcessarItensPedidoCorretamenteSemPesoESemTamanho()
    {
        var primeiroProduto = new ItensPedidoModel()
        {
            ProdutoId = Guid.NewGuid(),
            Quantidade = 5,
            ValorUnitario = 187
        };
        var segundoProduto = new ItensPedidoModel()
        {
            ProdutoId = Guid.NewGuid(),
            Quantidade = 2,
            ValorUnitario = 947
        };

        var itensPedidoModel = new List<ItensPedidoModel>()
        {
           primeiroProduto,
           segundoProduto
        };
        var pedido = PedidoBuilder.Init().Build();

        pedido.ProcessarItensPedido(itensPedidoModel);


        var item1 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == primeiroProduto.ProdutoId);

        var item2 = pedido
            .ItensPedido
            .FirstOrDefault(x => x.ProdutoId == segundoProduto.ProdutoId);


        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.Equal(primeiroProduto.ValorUnitario, item1.ValorUnitario);
        Assert.Equal(segundoProduto.ValorUnitario, item2.ValorUnitario);
        Assert.Equal(primeiroProduto.Quantidade, item1.Quantidade);
        Assert.Equal(segundoProduto.Quantidade, item2.Quantidade);

    }
}
