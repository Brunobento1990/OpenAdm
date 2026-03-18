namespace OpenAdm.Application.Models.Pedidos;

public class CriarPedidoViewModel
{
    public PedidoViewModel Pedido { get; set; } = null!;
    public string Message { get; set; } = string.Empty;
    public string? Redirect { get; set; }
}