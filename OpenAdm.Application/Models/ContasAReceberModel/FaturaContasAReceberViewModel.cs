using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.ContasAReceberModel;

public class FaturaContasAReceberViewModel : BaseViewModel
{
    public StatusFaturaContasAReceberEnum Status { get; set; }
    public DateTime DataDeVencimento { get; set; }
    public DateTime? DataDePagamento { get; set; }
    public int NumeroDaFatura { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
    public decimal? Desconto { get; set; }
    public string? Observacao { get; set; }
    public Guid ContasAReceberId { get; set; }
    public bool Vencida { get; set; }
    public ContasAReceberViewModel ContasAReceber { get; set; } = null!;

    public static explicit operator FaturaContasAReceberViewModel(FaturaContasAReceber faturaContasAReceber)
    {
        if (faturaContasAReceber.ContasAReceber != null)
        {
            faturaContasAReceber.ContasAReceber.Faturas = [];
        }

        return new FaturaContasAReceberViewModel()
        {
            ContasAReceber = faturaContasAReceber.ContasAReceber != null ? (ContasAReceberViewModel)faturaContasAReceber.ContasAReceber : null!,
            ContasAReceberId = faturaContasAReceber.ContasAReceberId,
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
