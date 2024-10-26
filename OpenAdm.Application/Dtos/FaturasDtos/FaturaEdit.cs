using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public class FaturaEdit
{
    public Guid Id { get; set; }
    public StatusParcelaEnum Status { get; set; }
    public DateTime DataDeVencimento { get; set; }
    public DateTime? DataDePagamento { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
    public decimal? Desconto { get; set; }
    public string? Observacao { get; set; }
}
