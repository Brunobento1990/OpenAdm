using OpenAdm.Application.Dtos.Pedidos;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface ICreatePedidoAdmService
{
    Task<bool> CreateAsync(PedidoAdmCreateDto pedidoAdmCreateDto);
}
