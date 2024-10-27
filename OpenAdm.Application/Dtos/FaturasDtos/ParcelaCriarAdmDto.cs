using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public class ParcelaCriarAdmDto
{
    public DateTime DataDeVencimento { get; set; }
    public int NumeroDaFatura { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public decimal Valor { get; set; }
    public decimal? Desconto { get; set; }
    public string? Observacao { get; set; }
}
