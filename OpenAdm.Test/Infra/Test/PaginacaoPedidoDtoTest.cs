using OpenAdm.Infra.Paginacao;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Infra.Test;

public sealed class PaginacaoPedidoDtoTest
{
    [Fact]
    public void DeveFiltrarPedidosPeloRepresentante()
    {
        var representante = RepresentanteBuilder.Init().Build();
        var pedidoDoRepresentante = PedidoBuilder.Init()
            .ComRepresentante(representante)
            .Build();
        var pedidoDeOutroRepresentante = PedidoBuilder.Init().Build();
        var filtro = new PaginacaoPedidoDto { RepresentanteId = representante.Id };

        var expressao = filtro.GetWhereBySearch();

        Assert.NotNull(expressao);
        Assert.True(expressao.Compile()(pedidoDoRepresentante));
        Assert.False(expressao.Compile()(pedidoDeOutroRepresentante));
    }
}
