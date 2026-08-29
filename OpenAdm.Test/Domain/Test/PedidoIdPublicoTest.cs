using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Domain.Test;

public class PedidoIdPublicoTest
{
    [Fact]
    public void DeveGerarIdentificadorPublicoNaoSequencialAoCriarPedido()
    {
        var primeiro = PedidoBuilder.Init().Build();
        var segundo = PedidoBuilder.Init().Build();

        Assert.NotEqual(Guid.Empty, primeiro.IdPublico);
        Assert.NotEqual(primeiro.IdPublico, segundo.IdPublico);
    }
}
