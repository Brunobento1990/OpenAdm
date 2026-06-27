namespace OpenAdm.Domain.Model.Pedidos;

public class ItemCobrancaHomeAdmModel
{
    public Guid PedidoId { get; set; }
    public long NumeroPedido { get; set; }
    public string Cliente { get; set; } = string.Empty;
}