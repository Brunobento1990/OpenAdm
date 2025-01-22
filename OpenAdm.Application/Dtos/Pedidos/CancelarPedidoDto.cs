namespace OpenAdm.Application.Dtos.Pedidos;

public class CancelarPedidoDto
{
    public Guid PedidoId { get; set; }
    public string? Motivo { get; set; }
}
