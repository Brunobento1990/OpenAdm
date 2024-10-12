using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.ContasAReceberDto;

public class PagarFaturaAReceberDto
{
    public Guid Id { get; set; }
    public decimal? Desconto { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public string? Observacao { get; set; }
}
