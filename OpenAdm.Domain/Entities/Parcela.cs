using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities;

public sealed class Parcela : BaseEntity
{
    public Parcela(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        DateTime dataDeVencimento,
        int numeroDaParcela,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        string? observacao,
        Guid faturaId,
        string? idExterno,
        decimal? desconto)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        DataDeVencimento = dataDeVencimento;
        NumeroDaParcela = numeroDaParcela;
        MeioDePagamento = meioDePagamento;
        Valor = valor;
        Observacao = observacao;
        FaturaId = faturaId;
        IdExterno = idExterno;
        Desconto = desconto;
    }

    public DateTime DataDeVencimento { get; private set; }
    public int NumeroDaParcela { get; private set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; private set; }
    public decimal Valor { get; private set; }
    public decimal? Desconto { get; private set; }
    public string? Observacao { get; private set; }
    public string? IdExterno { get; private set; }
    public Guid FaturaId { get; private set; }
    public StatusParcelaEnum Status
    {
        get
        {
            if (ValorPagoRecebido >= Valor)
            {
                return StatusParcelaEnum.Pago;
            }

            if (ValorPagoRecebido > 0)
            {
                return StatusParcelaEnum.Pago_Parcial;
            }

            return StatusParcelaEnum.Pendente;
        }
    }
    public Fatura Fatura { get; set; } = null!;
    public decimal ValorAPagarAReceber
    {
        get
        {
            var desconto = Desconto ?? 0;
            var valor = (Valor - desconto) - ValorPagoRecebido;
            return valor < 0 ? 0 : valor;
        }
    }
    public decimal ValorPagoRecebido
    {
        get
        {
            if (Transacoes == null || Transacoes.Count == 0)
            {
                return 0;
            }

            Func<TransacaoFinanceira, bool> wherePagosRecebidos =
                Fatura.Tipo == TipoFaturaEnum.A_Pagar ? x => x.TipoTransacaoFinanceira == TipoTransacaoFinanceiraEnum.Saida
                    : x => x.TipoTransacaoFinanceira == TipoTransacaoFinanceiraEnum.Entrada;
            Func<TransacaoFinanceira, bool> whereEstorno =
                Fatura.Tipo == TipoFaturaEnum.A_Pagar ? x => x.TipoTransacaoFinanceira == TipoTransacaoFinanceiraEnum.Entrada
                : x => x.TipoTransacaoFinanceira == TipoTransacaoFinanceiraEnum.Saida;

            var totalTransacoesPagos = Transacoes.Where(wherePagosRecebidos)
                .Sum(x => x.Valor);
            var totalTransacoesEstorno = Transacoes.Where(whereEstorno)
                .Sum(x => x.Valor);

            var desconto = Desconto ?? 0;

            return (totalTransacoesPagos - desconto) - totalTransacoesEstorno;
        }
    }

    public bool Vencida { get => DataDeVencimento.Date < DateTime.Now.Date; }
    public IList<TransacaoFinanceira>? Transacoes { get; set; }

    public void Edit(
        DateTime dataDeVencimento,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        decimal? desconto,
        string? observacao)
    {
        DataDeVencimento = dataDeVencimento;
        MeioDePagamento = meioDePagamento;
        Valor = valor;
        Desconto = desconto;
        Observacao = observacao;
    }
    public void TirarDiferenca(decimal diferenca)
    {
        Valor -= diferenca;
    }
    public void AdicionarDiferenca(decimal diferenca)
    {
        Valor += diferenca;
    }

    public TransacaoFinanceira Pagar(
        decimal valor,
        MeioDePagamentoEnum? meioDePagamento,
        string? observacao,
        DateTime? dataDePagamento,
        decimal? desconto)
    {
        Desconto = desconto;

        return TransacaoFinanceira.NovaTransacao(
            parcelaId: Id,
            dataDePagamento: dataDePagamento,
            valor: valor,
            tipoTransacaoFinanceira: Fatura.Tipo == TipoFaturaEnum.A_Pagar ? TipoTransacaoFinanceiraEnum.Saida : TipoTransacaoFinanceiraEnum.Entrada,
            meioDePagamento: meioDePagamento,
            observacao: observacao ?? $"Pagamento da parcela: {NumeroDaParcela}");
    }

    public TransacaoFinanceira Estornar()
    {
        return TransacaoFinanceira.NovaTransacao(
            parcelaId: Id,
            dataDePagamento: DateTime.Now,
            valor: ValorPagoRecebido,
            tipoTransacaoFinanceira: Fatura.Tipo == TipoFaturaEnum.A_Pagar ? TipoTransacaoFinanceiraEnum.Entrada : TipoTransacaoFinanceiraEnum.Saida,
            meioDePagamento: null,
            observacao: $"Estorno da parcela: {NumeroDaParcela}");
    }

    public static Parcela NovaFatura(
        DateTime dataDeVencimento,
        int numeroDaParcela,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        string? observacao,
        Guid faturaId,
        string? idExterno,
        decimal? desconto
        )
    {
        return new Parcela(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            dataDeVencimento: dataDeVencimento,
            numeroDaParcela: numeroDaParcela,
            meioDePagamento: meioDePagamento,
            valor: valor,
            observacao: observacao,
            faturaId: faturaId,
            idExterno: idExterno,
            desconto: desconto);
    }
}
