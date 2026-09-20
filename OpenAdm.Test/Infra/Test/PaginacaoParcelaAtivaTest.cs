using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Test.Infra.Test;

public class PaginacaoParcelaAtivaTest
{
    [Fact]
    public void DeveExcluirParcelaInativaDaConsultaSemBusca()
    {
        var fatura = new Fatura(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow,
            0, StatusFaturaEnum.Aberta, Guid.NewGuid(), null, null, TipoFaturaEnum.AReceber, 100);
        var ativa = Parcela.NovaFatura(DateTime.UtcNow, 1, null, 100, null,
            fatura.Id, null, null, null, fatura.Tipo);
        ativa.Fatura = fatura;
        var inativa = Parcela.NovaFatura(DateTime.UtcNow, 2, null, 100, null,
            fatura.Id, null, null, null, fatura.Tipo);
        inativa.Fatura = fatura;
        inativa.Inativar();
        var filtro = new PaginacaoParcelaDto { Tipo = TipoFaturaEnum.AReceber };
        var where = filtro.GetWhereBySearch().Compile();

        Assert.True(where(ativa));
        Assert.False(where(inativa));
    }
}
