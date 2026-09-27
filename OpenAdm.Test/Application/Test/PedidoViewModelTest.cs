using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public sealed class PedidoViewModelTest
{
    [Fact]
    public void DeveMapearRepresentanteETabelaDePrecoQuandoInformados()
    {
        var representante = RepresentanteBuilder.Init().ComNome("Representante do pedido").Build();
        var tabelaDePreco = TabelaDePrecoBuilder.Init().SemDescricao("Tabela especial").Build();
        var pedido = PedidoBuilder.Init()
            .ComRepresentante(representante)
            .ComTabelaDePreco(tabelaDePreco)
            .Build();

        var model = new PedidoViewModel().ForModel(pedido);

        Assert.Equal(representante.Id, model.Representante?.Id);
        Assert.Equal("Representante do pedido", model.Representante?.Nome);
        Assert.Equal(tabelaDePreco.Id, model.TabelaDePreco?.Id);
        Assert.Equal("Tabela especial", model.TabelaDePreco?.Descricao);
    }

    [Fact]
    public void DeveManterRepresentanteETabelaDePrecoNulosQuandoNaoInformados()
    {
        var pedido = PedidoBuilder.Init().Build();

        var model = new PedidoViewModel().ForModel(pedido);

        Assert.Null(model.Representante);
        Assert.Null(model.TabelaDePreco);
    }
}
