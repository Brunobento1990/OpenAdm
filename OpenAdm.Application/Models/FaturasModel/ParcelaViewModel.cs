using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.FaturasModel;

public class ParcelaViewModel : BaseViewModel
{
    public StatusParcelaEnum Status { get; set; }
    public DateTime DataDeVencimento { get; set; }
    public DateTime? DataDePagamento { get; set; }
    public int NumeroDaFatura { get; set; }
    public long NumeroDoPedido { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
    public decimal? Desconto { get; set; }
    public string? Observacao { get; set; }
    public Guid FaturaId { get; set; }
    public bool Vencida { get; set; }
    public FaturaViewModel Fatura { get; set; } = null!;

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
            NumeroDaFatura = faturaContasAReceber.NumeroDaFatura,
            Observacao = faturaContasAReceber.Observacao,
            Status = faturaContasAReceber.Status,
            Valor = faturaContasAReceber.Valor,
            Vencida = DateTime.Now.Date > faturaContasAReceber.DataDeVencimento.Date && faturaContasAReceber.DataDePagamento == null,
            DataDePagamento = faturaContasAReceber.DataDePagamento
        };
    }
}
