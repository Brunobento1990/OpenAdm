using OpenAdm.Domain.Exceptions;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Domain.Test;

public sealed class ParcelaTest
{
    [Fact]
    public void NaoDeveRealizarOperacoesEmParcelaInativa()
    {
        var parcela = ParcelaBuilder.Init().Build();
        parcela.Inativar();

        Assert.Throws<ExceptionApi>(() => parcela.Edit(
            DateTime.UtcNow.AddDays(10), null, 50, null, null));
        Assert.Throws<ExceptionApi>(() => parcela.Pagar(
            50, null, null, DateTime.UtcNow, null, null));
        Assert.Throws<ExceptionApi>(() => parcela.Estornar());
        Assert.Throws<ExceptionApi>(() => parcela.ConsolidarBaixaParcial());
        Assert.Throws<ExceptionApi>(() => parcela.Inativar());
    }
}
