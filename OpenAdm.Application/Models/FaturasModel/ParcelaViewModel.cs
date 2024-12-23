using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.Transacoes;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.FaturasModel;

public class ParcelaViewModel : BaseViewModel
{
    public DateTime DataDeVencimento { get; set; }
    public int NumeroDaParcela { get; set; }
    public long NumeroDoPedido { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorPagoRecebido { get; set; }
    public decimal ValorAPagarAReceber { get; set; }
    public decimal? Desconto { get; set; }
    public string? Observacao { get; set; }
    public bool Vencida { get; set; }
    public Guid FaturaId { get; set; }
    public StatusParcelaEnum Status { get; set; }
    public FaturaViewModel Fatura { get; set; } = null!;
    public IList<TransacaoFinanceiraViewModel>? Transacoes { get; set; }

    public static explicit operator ParcelaViewModel(Parcela faturaContasAReceber)
    {
        if (faturaContasAReceber.Fatura != null)
        {
            faturaContasAReceber.Fatura.Parcelas = [];
        }

        return new ParcelaViewModel()
        {
            Fatura = faturaContasAReceber.Fatura != null ? (FaturaViewModel)faturaContasAReceber.Fatura : null!,
            FaturaId = faturaContasAReceber.FaturaId,
            DataDeCriacao = faturaContasAReceber.DataDeCriacao,
            DataDeVencimento = faturaContasAReceber.DataDeVencimento,
            Desconto = faturaContasAReceber.Desconto,
            Id = faturaContasAReceber.Id,
            MeioDePagamento = faturaContasAReceber.MeioDePagamento,
            Numero = faturaContasAReceber.Numero,
            NumeroDaParcela = faturaContasAReceber.NumeroDaParcela,
            Observacao = faturaContasAReceber.Observacao,
            Valor = faturaContasAReceber.Valor,
            Status = faturaContasAReceber.Status,
            ValorAPagarAReceber = faturaContasAReceber.ValorAPagarAReceber,
            ValorPagoRecebido = faturaContasAReceber.ValorPagoRecebido,
            Vencida = faturaContasAReceber.Vencida,
            Transacoes = faturaContasAReceber.Transacoes?.Select(x =>
            {
                x.Parcela = null;
                return (TransacaoFinanceiraViewModel)x;
            }).ToList()
        };
    }
}
