using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Dtos.Pedidos;

public class PedidoAdmCreateDto
{
    public Guid UsuarioId { get; set; }
    public IList<ItemPedidoModel> Itens { get; set; } = [];
}

