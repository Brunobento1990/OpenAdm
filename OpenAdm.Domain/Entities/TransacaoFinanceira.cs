using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities;

public sealed class TransacaoFinanceira : BaseEntity
{
    public TransacaoFinanceira(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        Guid? parcelaId,
        DateTime dataDePagamento,
        decimal valor,
        TipoTransacaoFinanceiraEnum tipoTransacaoFinanceira,
        MeioDePagamentoEnum? meioDePagamento,
        string? observacao)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ParcelaId = parcelaId;
        DataDePagamento = dataDePagamento;
        Valor = valor;
        TipoTransacaoFinanceira = tipoTransacaoFinanceira;
        MeioDePagamento = meioDePagamento;
        Observacao = observacao;
    }

    public Guid? ParcelaId { get; private set; }
    public Parcela? Parcela { get; set; }
    public DateTime DataDePagamento { get; private set; }
    public decimal Valor { get; private set; }
    public TipoTransacaoFinanceiraEnum TipoTransacaoFinanceira { get; private set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; private set; }
    public string? Observacao { get; private set; }

    public static TransacaoFinanceira NovaTransacao(
        Guid? parcelaId,
        DateTime? dataDePagamento,
        decimal valor,
        TipoTransacaoFinanceiraEnum tipoTransacaoFinanceira,
        MeioDePagamentoEnum? meioDePagamento,
        string? observacao
        )
    {
        if (dataDePagamento.HasValue)
        {
            dataDePagamento.Value.AddHours(DateTime.Now.Hour);
            dataDePagamento.Value.AddMinutes(DateTime.Now.Minute);
        }

        return new TransacaoFinanceira(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            parcelaId: parcelaId,
            dataDePagamento: dataDePagamento ?? DateTime.Now,
            valor: valor,
            tipoTransacaoFinanceira: tipoTransacaoFinanceira,
            meioDePagamento: meioDePagamento,
            observacao: observacao);
    }
}
