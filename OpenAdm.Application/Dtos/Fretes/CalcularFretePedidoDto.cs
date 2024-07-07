namespace OpenAdm.Application.Dtos.Fretes;

public class CalcularFretePedidoDto
{
    public Guid PedidoId { get; set; }
    public string Cep { get; set; } = string.Empty;
    public string TipoFrete { get; set; } = string.Empty;
}
