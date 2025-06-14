using OpenAdm.Application.Dtos.EnderecosDeEntregasPedidos;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Dtos.Pedidos;

public class PedidoAdmCreateDto
{
    public Guid UsuarioId { get; set; }
    public IList<ItemPedidoModel> ItensPedido { get; set; } = [];
    public EnderecoEntregaPedidoCreateDto? EnderecoEntrega { get; set; }
}

