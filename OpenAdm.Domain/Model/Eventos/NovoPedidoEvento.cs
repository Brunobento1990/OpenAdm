namespace OpenAdm.Domain.Model.Eventos;

public class NovoPedidoEvento : EventoBase
{
    public Guid PedidoId { get; set; }
}