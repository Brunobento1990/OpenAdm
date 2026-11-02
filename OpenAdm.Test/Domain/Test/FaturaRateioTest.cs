using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Test.Domain.Test;

public sealed class FaturaRateioTest
{
    [Fact]
    public void DeveRatearDiferencaDeCentavosNaUltimaParcela()
    {
        var fatura = CriarFatura();
        AdicionarParcela(fatura, 1, 33.33m);
        AdicionarParcela(fatura, 2, 66.67m);

        fatura.AjustarValorAReceber(100.01m);

        Assert.Equal(100.01m, fatura.Total);
        Assert.Equal(33.33m, fatura.Parcelas[0].Valor);
        Assert.Equal(66.68m, fatura.Parcelas[1].Valor);
        Assert.Equal(100.01m, fatura.ValorAPagarAReceber);
    }

    [Fact]
    public void DeveRatearSomenteOValorAReceberPreservandoOValorPago()
    {
        var fatura = CriarFatura();
        var primeira = AdicionarParcela(fatura, 1, 50m);
        primeira.Transacoes = [primeira.Pagar(10m, null, null, DateTime.UtcNow, null, null)];
        var segunda = AdicionarParcela(fatura, 2, 50m);

        fatura.AjustarValorAReceber(90m);

        Assert.Equal(10m, primeira.ValorPagoRecebido);
        Assert.Equal(45.56m, primeira.Valor);
        Assert.Equal(44.44m, segunda.Valor);
        Assert.Equal(80m, fatura.ValorAPagarAReceber);
    }

    private static Fatura CriarFatura()
        => new(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 0,
            StatusFaturaEnum.Aberta, Guid.NewGuid(), null, null,
            TipoFaturaEnum.AReceber, 100m);

    private static Parcela AdicionarParcela(Fatura fatura, int numero, decimal valor)
    {
        var parcela = Parcela.NovaFatura(DateTime.UtcNow.Date, numero, null, valor,
            null, fatura.Id, null, null, null, fatura.Tipo);
        parcela.Fatura = fatura;
        fatura.Parcelas.Add(parcela);
        return parcela;
    }
}
