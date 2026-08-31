using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

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
        decimal? desconto, TipoFaturaEnum tipo, bool quitada, decimal? juros)
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
        Tipo = tipo;
        Quitada = quitada;
        Juros = juros;
    }

    public DateTime DataDeVencimento { get; private set; }
    public int NumeroDaParcela { get; private set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; private set; }
    public decimal Valor { get; private set; }
    public decimal? Desconto { get; private set; }
    public decimal? Juros { get; private set; }
    public string? Observacao { get; private set; }
    public string? IdExterno { get; private set; }
    public TipoFaturaEnum Tipo { get; private set; }
    public Guid FaturaId { get; private set; }
    public bool Quitada { get; private set; }

    public StatusParcelaEnum Status
    {
        get
        {
            if (Quitada)
            {
                return StatusParcelaEnum.Pago;
            }
            
            if (Vencida)
            {
                return StatusParcelaEnum.Vencida;
            }

            if (ValorPagoRecebido > 0)
            {
                return StatusParcelaEnum.PagoParcial;
            }

            return StatusParcelaEnum.Pendente;
        }
    }

    public Fatura Fatura { get; set; } = null!;

    public decimal ValorAPagarAReceber => Valor - ValorPagoRecebido;

    public decimal ValorPagoRecebido =>
        CalculoParcelaHelper.CalcularValorPagoRecebido(Tipo, Transacoes?.Cast<ITransacaoParaCalculo>());

    public decimal ValorPagoRecebidoLiquido =>
        CalculoParcelaHelper.CalcularValorPagoRecebidoLiquido(Tipo, Transacoes?.Cast<ITransacaoParaCalculo>());

    public decimal DescontoConcedido =>
        CalculoParcelaHelper.CalcularDescontoConcedido(Tipo, Transacoes?.Cast<ITransacaoParaCalculo>());

    public bool Vencida
    {
        get => Quitada ? false : DataDeVencimento.Date < DateTime.Now.Date;
    }

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

    public TransacaoFinanceira Pagar(
        decimal valor,
        MeioDePagamentoEnum? meioDePagamento,
        string? observacao,
        DateTime? dataDePagamento,
        decimal? desconto,
        decimal? juros)
    {
        if (Quitada)
        {
            throw new ExceptionApi($"A parcela: {NumeroDaParcela} já se encontra paga");
        }

        Quitada = (ValorPagoRecebido + valor + (desconto ?? 0) - (juros ?? 0)) >= Valor;

        return TransacaoFinanceira.NovaTransacao(
            parcelaId: Id,
            dataDeEfetivacao: dataDePagamento,
            valor: valor,
            tipoTransacaoFinanceira: Tipo == TipoFaturaEnum.APagar
                ? TipoTransacaoFinanceiraEnum.Saida
                : TipoTransacaoFinanceiraEnum.Entrada,
            meioDePagamento: meioDePagamento,
            observacao: observacao ?? $"Pagamento da parcela: {NumeroDaParcela}",
            foiEstornado: false,
            juros: juros,
            desconto: desconto);
    }

    public IList<TransacaoFinanceira> Estornar()
    {
        var tipoTransacaoDePagamento = Tipo == TipoFaturaEnum.APagar
            ? TipoTransacaoFinanceiraEnum.Saida
            : TipoTransacaoFinanceiraEnum.Entrada;

        var transacoesParaEstornar = Transacoes?
            .Where(x => x.FoiEstornado != true &&
                        x.TipoTransacaoFinanceira == tipoTransacaoDePagamento)
            .ToList() ?? [];

        if (transacoesParaEstornar.Count == 0)
        {
            throw new ExceptionApi("Não há transações para estornar!");
        }

        var estornos = new List<TransacaoFinanceira>();

        foreach (var transacao in transacoesParaEstornar)
        {
            var estorno = transacao.Estornar();
            estornos.Add(estorno);
        }

        Quitada = false;
        DataDeAtualizacao = DateTime.UtcNow;

        return estornos;
    }

    public static Parcela NovaFatura(
        DateTime dataDeVencimento,
        int numeroDaParcela,
        MeioDePagamentoEnum? meioDePagamento,
        decimal valor,
        string? observacao,
        Guid faturaId,
        string? idExterno,
        decimal? desconto,
        decimal? juros,
        TipoFaturaEnum tipoFatura
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
            desconto: desconto,
            tipo: tipoFatura,
            quitada: false,
            juros: juros);
    }
}