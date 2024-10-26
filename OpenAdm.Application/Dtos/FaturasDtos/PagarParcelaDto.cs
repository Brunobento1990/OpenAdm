using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.FaturasDtos;

public class PagarParcelaDto
{
    public Guid Id { get; set; }
    public decimal? Desconto { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public string? Observacao { get; set; }
}
