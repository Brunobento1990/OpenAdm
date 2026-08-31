using OpenAdm.Application.Dtos.TransacoesFinanceiras;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class TransacaoFinanceiraServiceTest
{
    [Fact]
    public async Task TransacoesNoPeriodoAsync_DeveAgruparPorDiaEOrdenarComoExtrato()
    {
        var dataMaisAntiga = new DateTime(2026, 8, 29, 10, 30, 0);
        var dataMaisRecente = new DateTime(2026, 8, 30, 9, 0, 0);
        var dataMaisRecenteNoMesmoDia = new DateTime(2026, 8, 30, 17, 15, 0);
        var transacoes = new List<TransacaoFinanceira>
        {
            CriarTransacao(dataMaisAntiga),
            CriarTransacao(dataMaisRecente),
            CriarTransacao(dataMaisRecenteNoMesmoDia)
        };
        var repository = new Mock<ITransacaoFinanceiraRepository>();
        repository
            .Setup(x => x.TransacoesNoPeriodoAsync(
                new DateTime(2026, 8, 1),
                new DateTime(2026, 8, 31),
                null,
                null))
            .ReturnsAsync(transacoes);
        var service = new TransacaoFinanceiraService(repository.Object);
        var filtro = new TransacaoFinanceiraNoPeriodoDto
        {
            DataInicial = new DateTime(2026, 8, 1),
            DataFinal = new DateTime(2026, 8, 31)
        };

        var resultado = await service.TransacoesNoPeriodoAsync(filtro);

        Assert.Equal(2, resultado.Count);
        Assert.Equal(
            [new DateTime(2026, 8, 30), new DateTime(2026, 8, 29)],
            resultado.Keys);
        Assert.Equal(
            [dataMaisRecenteNoMesmoDia, dataMaisRecente],
            resultado[new DateTime(2026, 8, 30)].Transacoes.Select(x => x.DataDeEfetivacao));
        Assert.Equal(200, resultado[new DateTime(2026, 8, 30)].Total);
        Assert.Equal(100, resultado[new DateTime(2026, 8, 29)].Total);
    }

    private static TransacaoFinanceira CriarTransacao(DateTime dataDeEfetivacao)
    {
        return new TransacaoFinanceira(
            Guid.NewGuid(),
            dataDeEfetivacao,
            dataDeEfetivacao,
            0,
            null,
            dataDeEfetivacao,
            100,
            TipoTransacaoFinanceiraEnum.Entrada,
            null,
            null,
            false,
            null,
            null);
    }
}
