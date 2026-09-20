using Moq;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class FaturaRenegociacaoServiceTest
{
    [Theory]
    [InlineData(TipoFaturaEnum.AReceber)]
    [InlineData(TipoFaturaEnum.APagar)]
    public async Task DeveSugerirFaturaComSomenteParcelasAtivasComSaldo(TipoFaturaEnum tipo)
    {
        var fatura = new Fatura(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow,
            0, StatusFaturaEnum.Aberta, Guid.NewGuid(), null, null, tipo, 150);
        var parcial = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 100, null,
            fatura.Id, null, null, null, tipo);
        parcial.Fatura = fatura;
        parcial.Transacoes =
        [
            TransacaoFinanceira.NovaTransacao(parcial.Id, DateTime.UtcNow, 30,
                tipo == TipoFaturaEnum.APagar ? TipoTransacaoFinanceiraEnum.Saida
                    : TipoTransacaoFinanceiraEnum.Entrada, null, null, false, null, null)
        ];
        var quitada = Parcela.NovaFatura(DateTime.UtcNow.Date, 2, null, 30, null,
            fatura.Id, null, null, null, tipo);
        quitada.Fatura = fatura;
        quitada.Transacoes = [quitada.Pagar(30, null, null, DateTime.UtcNow, null, null)];
        var inativa = Parcela.NovaFatura(DateTime.UtcNow.Date, 3, null, 20, null,
            fatura.Id, null, null, null, tipo);
        inativa.Fatura = fatura;
        inativa.Inativar();
        fatura.Parcelas.Add(parcial);
        fatura.Parcelas.Add(quitada);
        fatura.Parcelas.Add(inativa);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.GetByIdCompletaAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).SugerirParcelamentoAsync(fatura.Id);

        Assert.Null(resultado.Error);
        Assert.Equal(fatura.Id, resultado.Result!.Id);
        Assert.Equal(tipo, resultado.Result.Tipo);
        Assert.Equal(150, resultado.Result.Total);
        Assert.Single(resultado.Result.Parcelas);
        Assert.Equal(parcial.Id, resultado.Result.Parcelas[0].Id);
        Assert.Equal(70, resultado.Result.Parcelas[0].ValorAPagarAReceber);
        Assert.Equal(3, fatura.Parcelas.Count);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeveRetornarFaturaSemParcelasQuandoNaoHaSaldo()
    {
        var fatura = CriarFatura(30);
        var quitada = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 30, null,
            fatura.Id, null, null, null, fatura.Tipo);
        quitada.Fatura = fatura;
        quitada.Transacoes = [quitada.Pagar(30, null, null, DateTime.UtcNow, null, null)];
        fatura.Parcelas.Add(quitada);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.GetByIdCompletaAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).SugerirParcelamentoAsync(fatura.Id);

        Assert.Null(resultado.Error);
        Assert.Equal(fatura.Id, resultado.Result!.Id);
        Assert.Empty(resultado.Result.Parcelas);
    }

    [Fact]
    public async Task DeveRejeitarIdVazioSemConsultarFaturaParaSugestao()
    {
        var repository = new Mock<IFaturaRepository>();

        var resultado = await CriarServico(repository.Object).SugerirParcelamentoAsync(Guid.Empty);

        Assert.Equal("Informe a fatura!", resultado.Error);
        repository.Verify(x => x.GetByIdCompletaAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeveConsolidarBaixaParcialECriarParcelaParaSaldo()
    {
        var fatura = CriarFatura(100);
        var antiga = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 100, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        antiga.Transacoes =
        [
            TransacaoFinanceira.NovaTransacao(antiga.Id, DateTime.UtcNow, 30,
                TipoTransacaoFinanceiraEnum.Entrada, null, null, false, null, null)
        ];
        fatura.Parcelas.Add(antiga);

        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);
        var novas = new List<Parcela>();
        repository.Setup(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()))
            .Callback<IEnumerable<Parcela>>(x => novas.AddRange(x)).Returns(Task.CompletedTask);
        FaturaHistorico? historicoAdicionado = null;
        repository.Setup(x => x.AddHistoricoAsync(It.IsAny<FaturaHistorico>()))
            .Callback<FaturaHistorico>(x => historicoAdicionado = x)
            .Returns(Task.CompletedTask);

        var service = CriarServico(repository.Object);
        var resultado = await service.RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 2, Valor = 70, DataDeVencimento = DateTime.UtcNow.Date.AddDays(30) }]
        });

        Assert.Null(resultado.Error);
        Assert.True(antiga.Ativo);
        Assert.True(antiga.Quitada);
        Assert.Equal(30, antiga.Valor);
        Assert.Single(novas);
        Assert.Equal(70, novas[0].Valor);
        Assert.Equal(fatura.Id, novas[0].FaturaId);
        Assert.NotNull(historicoAdicionado);
        Assert.Equal(fatura.Id, historicoAdicionado.FaturaId);
        Assert.Equal(
            "Fatura renegociada: 1 parcela(s) criada(s), 0 parcela(s) inativada(s) e 1 baixa(s) parcial(is) consolidada(s).",
            historicoAdicionado.Descricao);
        repository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeveRenumerarNovaParcelaQueUsaNumeroDeBaixaParcialPreservada()
    {
        var fatura = CriarFatura(100);
        var antiga = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 100, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        antiga.Transacoes =
        [
            TransacaoFinanceira.NovaTransacao(antiga.Id, DateTime.UtcNow, 30,
                TipoTransacaoFinanceiraEnum.Entrada, null, null, false, null, null)
        ];
        fatura.Parcelas.Add(antiga);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);
        var novas = new List<Parcela>();
        repository.Setup(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()))
            .Callback<IEnumerable<Parcela>>(x => novas.AddRange(x)).Returns(Task.CompletedTask);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 1, Valor = 70, DataDeVencimento = DateTime.UtcNow.Date.AddDays(30) }]
        });

        Assert.Null(resultado.Error);
        Assert.Equal(30, antiga.Valor);
        Assert.True(antiga.Quitada);
        Assert.True(antiga.Ativo);
        Assert.Single(novas);
        Assert.Equal(2, novas[0].NumeroDaParcela);
        repository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeveRenumerarNovasParcelasEmOrdemSemRepetirNumeroPreservado()
    {
        var fatura = CriarFatura(100);
        var antiga = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 100, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        antiga.Transacoes =
        [
            TransacaoFinanceira.NovaTransacao(antiga.Id, DateTime.UtcNow, 30,
                TipoTransacaoFinanceiraEnum.Entrada, null, null, false, null, null)
        ];
        fatura.Parcelas.Add(antiga);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);
        var novas = new List<Parcela>();
        repository.Setup(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()))
            .Callback<IEnumerable<Parcela>>(x => novas.AddRange(x)).Returns(Task.CompletedTask);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas =
            [
                new() { NumeroDaParcela = 1, Valor = 35, DataDeVencimento = DateTime.UtcNow.Date.AddDays(30) },
                new() { NumeroDaParcela = 2, Valor = 35, DataDeVencimento = DateTime.UtcNow.Date.AddDays(60) }
            ]
        });

        Assert.Null(resultado.Error);
        Assert.Equal([2, 3], novas.Select(x => x.NumeroDaParcela).ToArray());
        Assert.Equal(30, antiga.Valor);
    }

    [Fact]
    public async Task NaoDeveGravarQuandoParcelaNaoMudou()
    {
        var fatura = CriarFatura(100);
        var vencimento = DateTime.UtcNow.Date.AddDays(30);
        fatura.Parcelas.Add(Parcela.NovaFatura(vencimento, 1, null, 100, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber));
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 1, Valor = 100, DataDeVencimento = vencimento }]
        });

        Assert.Null(resultado.Error);
        repository.Verify(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Theory]
    [InlineData(69.99)]
    [InlineData(70.01)]
    public async Task NaoDeveAlterarBaixaParcialQuandoSaldoInformadoNaoTotaliza(decimal saldo)
    {
        var fatura = CriarFatura(100);
        var antiga = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 100, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        antiga.Transacoes =
        [
            TransacaoFinanceira.NovaTransacao(antiga.Id, DateTime.UtcNow, 30,
                TipoTransacaoFinanceiraEnum.Entrada, null, null, false, null, null)
        ];
        fatura.Parcelas.Add(antiga);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 2, Valor = saldo, DataDeVencimento = DateTime.UtcNow.Date.AddDays(30) }]
        });

        Assert.Contains("soma", resultado.Error);
        Assert.Equal(100, antiga.Valor);
        Assert.False(antiga.Quitada);
        repository.Verify(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeveArredondarCadaNovaParcelaAntesDeTotalizar()
    {
        var fatura = CriarFatura(20.01m);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);
        var novas = new List<Parcela>();
        repository.Setup(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()))
            .Callback<IEnumerable<Parcela>>(x => novas.AddRange(x)).Returns(Task.CompletedTask);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas =
            [
                new() { NumeroDaParcela = 1, Valor = 10.005m, DataDeVencimento = DateTime.UtcNow.Date },
                new() { NumeroDaParcela = 2, Valor = 9.995m, DataDeVencimento = DateTime.UtcNow.Date.AddDays(30) }
            ]
        });

        Assert.Null(resultado.Error);
        Assert.Equal([10.01m, 10.00m], novas.Select(x => x.Valor).ToArray());
        repository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task NaoDeveGravarValorComMaisCasasQuandoArredondadoNaoMuda()
    {
        var fatura = CriarFatura(100);
        var vencimento = DateTime.UtcNow.Date.AddDays(30);
        fatura.Parcelas.Add(Parcela.NovaFatura(vencimento, 1, null, 100, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber));
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 1, Valor = 100.004m, DataDeVencimento = vencimento }]
        });

        Assert.Null(resultado.Error);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DevePreservarParcelaIgualEInativarAAlterada()
    {
        var fatura = CriarFatura(100);
        var vencimento = DateTime.UtcNow.Date.AddDays(30);
        var igual = Parcela.NovaFatura(vencimento, 1, null, 50, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        var alterada = Parcela.NovaFatura(vencimento, 2, null, 50, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        fatura.Parcelas.Add(igual);
        fatura.Parcelas.Add(alterada);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);
        var novas = new List<Parcela>();
        repository.Setup(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()))
            .Callback<IEnumerable<Parcela>>(x => novas.AddRange(x)).Returns(Task.CompletedTask);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas =
            [
                new() { NumeroDaParcela = 1, Valor = 50, DataDeVencimento = vencimento },
                new() { NumeroDaParcela = 2, Valor = 50, DataDeVencimento = vencimento.AddDays(7) }
            ]
        });

        Assert.Null(resultado.Error);
        Assert.True(igual.Ativo);
        Assert.False(alterada.Ativo);
        Assert.Single(novas);
        Assert.Equal(2, novas[0].NumeroDaParcela);
        Assert.Equal(vencimento.AddDays(7), novas[0].DataDeVencimento);
    }

    [Fact]
    public async Task DeveManterValorPagoEParcelaSemAlteracaoAoDistribuirSaldo()
    {
        var fatura = CriarFatura(100);
        var vencimento = DateTime.UtcNow.Date.AddDays(30);
        var parcial = Parcela.NovaFatura(vencimento, 1, null, 50, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        parcial.Transacoes =
        [
            TransacaoFinanceira.NovaTransacao(parcial.Id, DateTime.UtcNow, 20,
                TipoTransacaoFinanceiraEnum.Entrada, null, null, false, null, null)
        ];
        var igual = Parcela.NovaFatura(vencimento, 2, null, 50, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        fatura.Parcelas.Add(parcial);
        fatura.Parcelas.Add(igual);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);
        var novas = new List<Parcela>();
        repository.Setup(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()))
            .Callback<IEnumerable<Parcela>>(x => novas.AddRange(x)).Returns(Task.CompletedTask);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas =
            [
                new() { NumeroDaParcela = 2, Valor = 50, DataDeVencimento = vencimento },
                new() { NumeroDaParcela = 3, Valor = 30, DataDeVencimento = vencimento.AddDays(30) }
            ]
        });

        Assert.Null(resultado.Error);
        Assert.Equal(20, parcial.Valor);
        Assert.True(parcial.Quitada);
        Assert.True(igual.Ativo);
        Assert.Single(novas);
        Assert.Equal(30, novas[0].Valor);
    }

    [Fact]
    public async Task NaoDeveGravarSeNovasParcelasNaoCompletamTotalPreservado()
    {
        var fatura = CriarFatura(100);
        var vencimento = DateTime.UtcNow.Date.AddDays(30);
        var igual = Parcela.NovaFatura(vencimento, 1, null, 50, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        var removida = Parcela.NovaFatura(vencimento, 2, null, 50, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        fatura.Parcelas.Add(igual);
        fatura.Parcelas.Add(removida);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas =
            [
                new() { NumeroDaParcela = 1, Valor = 50, DataDeVencimento = vencimento },
                new() { NumeroDaParcela = 3, Valor = 49.99m, DataDeVencimento = vencimento.AddDays(7) }
            ]
        });

        Assert.Contains("soma", resultado.Error);
        Assert.True(igual.Ativo);
        Assert.True(removida.Ativo);
        repository.Verify(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task NaoDeveSubstituirParcelaComIntegracaoExterna()
    {
        var fatura = CriarFatura(100);
        var vencimento = DateTime.UtcNow.Date.AddDays(30);
        var antiga = Parcela.NovaFatura(vencimento, 1, null, 100, null,
            fatura.Id, "pagamento-externo", null, null, TipoFaturaEnum.AReceber);
        fatura.Parcelas.Add(antiga);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 1, Valor = 100, DataDeVencimento = vencimento.AddDays(7) }]
        });

        Assert.Contains("integração externa", resultado.Error);
        Assert.True(antiga.Ativo);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DevePreservarParcelaQuitadaEValidarApenasSaldoPendente()
    {
        var fatura = CriarFatura(100);
        var vencimento = DateTime.UtcNow.Date.AddDays(30);
        var paga = Parcela.NovaFatura(vencimento, 1, null, 40, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        paga.Transacoes = [paga.Pagar(40, null, null, DateTime.UtcNow, null, null)];
        var pendente = Parcela.NovaFatura(vencimento, 2, null, 60, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        fatura.Parcelas.Add(paga);
        fatura.Parcelas.Add(pendente);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 2, Valor = 60, DataDeVencimento = vencimento }]
        });

        Assert.Null(resultado.Error);
        Assert.True(paga.Ativo);
        Assert.Equal(40, paga.Valor);
        Assert.True(pendente.Ativo);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeveSomarDuasBaixasParciaisAntesDeDistribuirSaldo()
    {
        var fatura = CriarFatura(100);
        var primeira = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 50, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        primeira.Transacoes =
        [
            TransacaoFinanceira.NovaTransacao(primeira.Id, DateTime.UtcNow, 20,
                TipoTransacaoFinanceiraEnum.Entrada, null, null, false, null, null)
        ];
        var segunda = Parcela.NovaFatura(DateTime.UtcNow.Date, 2, null, 50, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        segunda.Transacoes =
        [
            TransacaoFinanceira.NovaTransacao(segunda.Id, DateTime.UtcNow, 10,
                TipoTransacaoFinanceiraEnum.Entrada, null, null, false, null, null)
        ];
        fatura.Parcelas.Add(primeira);
        fatura.Parcelas.Add(segunda);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);
        var novas = new List<Parcela>();
        repository.Setup(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()))
            .Callback<IEnumerable<Parcela>>(x => novas.AddRange(x)).Returns(Task.CompletedTask);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas =
            [
                new() { NumeroDaParcela = 3, Valor = 35, DataDeVencimento = DateTime.UtcNow.Date.AddDays(30) },
                new() { NumeroDaParcela = 4, Valor = 35, DataDeVencimento = DateTime.UtcNow.Date.AddDays(60) }
            ]
        });

        Assert.Null(resultado.Error);
        Assert.Equal(20, primeira.Valor);
        Assert.Equal(10, segunda.Valor);
        Assert.True(primeira.Quitada);
        Assert.True(segunda.Quitada);
        Assert.Equal(70, novas.Sum(x => x.Valor));
    }

    [Fact]
    public async Task DeveIgnorarParcelaInativaAoValidarTotal()
    {
        var fatura = CriarFatura(100);
        var inativa = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 100, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        inativa.Inativar();
        fatura.Parcelas.Add(inativa);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);
        var novas = new List<Parcela>();
        repository.Setup(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()))
            .Callback<IEnumerable<Parcela>>(x => novas.AddRange(x)).Returns(Task.CompletedTask);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 1, Valor = 100, DataDeVencimento = DateTime.UtcNow.Date.AddDays(30) }]
        });

        Assert.Null(resultado.Error);
        Assert.False(inativa.Ativo);
        Assert.Single(novas);
        Assert.NotEqual(inativa.Id, novas[0].Id);
    }

    [Fact]
    public async Task DeveRejeitarFaturaComNumeroRepetidoEntreParcelaPagaEPendente()
    {
        var fatura = CriarFatura(100);
        var paga = Parcela.NovaFatura(DateTime.UtcNow.Date, 1, null, 30, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        paga.Transacoes = [paga.Pagar(30, null, null, DateTime.UtcNow, null, null)];
        var vencimento = DateTime.UtcNow.Date.AddDays(30);
        var pendente = Parcela.NovaFatura(vencimento, 1, null, 70, null,
            fatura.Id, null, null, null, TipoFaturaEnum.AReceber);
        fatura.Parcelas.Add(paga);
        fatura.Parcelas.Add(pendente);
        var repository = new Mock<IFaturaRepository>();
        repository.Setup(x => x.ObterParaRenegociarAsync(fatura.Id)).ReturnsAsync(fatura);

        var resultado = await CriarServico(repository.Object).RenegociarAsync(new RenegociarFaturaDto
        {
            FaturaId = fatura.Id,
            Parcelas = [new() { NumeroDaParcela = 1, Valor = 70, DataDeVencimento = vencimento }]
        });

        Assert.Contains("números repetidos", resultado.Error);
        Assert.True(paga.Ativo);
        Assert.True(pendente.Ativo);
        repository.Verify(x => x.AdicionarParcelasAsync(It.IsAny<IEnumerable<Parcela>>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    private static Fatura CriarFatura(decimal total) => new(Guid.NewGuid(), DateTime.UtcNow,
        DateTime.UtcNow, 0, StatusFaturaEnum.Aberta, Guid.NewGuid(), null, null,
        TipoFaturaEnum.AReceber, total);

    private static FaturaService CriarServico(IFaturaRepository repository) => new(repository,
        Mock.Of<IUsuarioService>(), Mock.Of<ICobrancaPedidoEcommerceRepository>(),
        Mock.Of<IPedidoRepository>(), Mock.Of<IParceiroAutenticado>());
}
