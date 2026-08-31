using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Domain.Entities;

public sealed class TransacaoFinanceira : BaseEntity, ITransacaoParaCalculo
{
    public TransacaoFinanceira(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        Guid? parcelaId,
        DateTime dataDeEfetivacao,
        decimal valor,
        TipoTransacaoFinanceiraEnum tipoTransacaoFinanceira,
        MeioDePagamentoEnum? meioDePagamento,
        string? observacao, bool? foiEstornado, decimal? desconto, decimal? juros)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ParcelaId = parcelaId;
        DataDeEfetivacao = dataDeEfetivacao;
        Valor = valor;
        TipoTransacaoFinanceira = tipoTransacaoFinanceira;
        MeioDePagamento = meioDePagamento;
        Observacao = observacao;
        FoiEstornado = foiEstornado;
        Desconto = desconto;
        Juros = juros;
    }

    public Guid? ParcelaId { get; private set; }
    public Parcela? Parcela { get; set; }
    public DateTime DataDeEfetivacao { get; private set; }
    public decimal Valor { get; private set; }
    public decimal? Desconto { get; private set; }
    public decimal? Juros { get; private set; }
    public bool? FoiEstornado { get; private set; }
    public TipoTransacaoFinanceiraEnum TipoTransacaoFinanceira { get; private set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; private set; }
    public string? Observacao { get; private set; }

    public bool EhEstorno
    {
        get
        {
            if (Parcela == null)
            {
                return false;
            }

            if (Parcela.Tipo == TipoFaturaEnum.APagar &&
                TipoTransacaoFinanceira == TipoTransacaoFinanceiraEnum.Entrada)
            {
                return true;
            }

            if (Parcela.Tipo == TipoFaturaEnum.AReceber &&
                TipoTransacaoFinanceira == TipoTransacaoFinanceiraEnum.Saida)
            {
                return true;
            }

            return false;
        }
    }

    public static TransacaoFinanceira NovaTransacao(
        Guid? parcelaId,
        DateTime? dataDeEfetivacao,
        decimal valor,
        TipoTransacaoFinanceiraEnum tipoTransacaoFinanceira,
        MeioDePagamentoEnum? meioDePagamento,
        string? observacao,
        bool? foiEstornado,
        decimal? juros,
        decimal? desconto)
    {
        var data = dataDeEfetivacao.HasValue ?
            new DateTime(
                dataDeEfetivacao.Value.Year,
                dataDeEfetivacao.Value.Month,
                dataDeEfetivacao.Value.Day,
                DateTime.UtcNow.Hour,
                DateTime.UtcNow.Minute,
                DateTime.UtcNow.Second) :
            DateTime.UtcNow;

        return new TransacaoFinanceira(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.UtcNow,
            dataDeAtualizacao: DateTime.UtcNow,
            numero: 0,
            parcelaId: parcelaId,
            dataDeEfetivacao: data,
            valor: valor,
            tipoTransacaoFinanceira: tipoTransacaoFinanceira,
            meioDePagamento: meioDePagamento,
            observacao: observacao,
            foiEstornado: foiEstornado,
            desconto: desconto,
            juros: juros);
    }

    public TransacaoFinanceira Estornar()
    {
        FoiEstornado = true;
        DataDeAtualizacao = DateTime.UtcNow;

        return new TransacaoFinanceira(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.UtcNow,
            dataDeAtualizacao: DateTime.UtcNow,
            numero: 0,
            parcelaId: ParcelaId,
            dataDeEfetivacao: DateTime.UtcNow,
            valor: Valor,
            tipoTransacaoFinanceira: TipoTransacaoFinanceira == TipoTransacaoFinanceiraEnum.Entrada
                ? TipoTransacaoFinanceiraEnum.Saida
                : TipoTransacaoFinanceiraEnum.Entrada,
            meioDePagamento: MeioDePagamento,
            observacao: "Estorno",
            foiEstornado: false,
            desconto: Desconto,
            juros: Juros);
    }
}
