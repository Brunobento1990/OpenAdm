using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Application.Models.FaturasModel;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.Transacoes;

public class TransacaoFinanceiraViewModel : BaseViewModel
{
    public Guid? ParcelaId { get; set; }
    public ParcelaViewModel? Parcela { get; set; }
    public DateTime DataDePagamento { get; set; }
    public decimal Valor { get; set; }
    public TipoTransacaoFinanceiraEnum TipoTransacaoFinanceira { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public string? Observacao { get; set; }

    public static explicit operator TransacaoFinanceiraViewModel(TransacaoFinanceira transacaoFinanceira)
    {
        return new TransacaoFinanceiraViewModel()
        {
            Id = transacaoFinanceira.Id,
            DataDeAtualizacao = transacaoFinanceira.DataDeAtualizacao,
            DataDeCriacao = transacaoFinanceira.DataDeCriacao,
            DataDePagamento = transacaoFinanceira.DataDePagamento,
            MeioDePagamento = transacaoFinanceira.MeioDePagamento,
            Numero = transacaoFinanceira.Numero,
            Observacao = transacaoFinanceira.Observacao,
            Parcela = transacaoFinanceira.Parcela == null ? null : (ParcelaViewModel)transacaoFinanceira.Parcela,
            ParcelaId = transacaoFinanceira.ParcelaId,
            TipoTransacaoFinanceira = transacaoFinanceira.TipoTransacaoFinanceira,
            Valor = transacaoFinanceira.Valor
        };
    }
}
