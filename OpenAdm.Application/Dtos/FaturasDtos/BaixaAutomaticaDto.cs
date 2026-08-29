namespace OpenAdm.Application.Dtos.FaturasDtos;

public sealed class BaixaAutomaticaDto
{
    public Guid PedidoId { get; set; }
    public decimal? Desconto { get; set; }
}
