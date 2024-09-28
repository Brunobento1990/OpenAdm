using OpenAdm.Domain.Entities;
namespace OpenAdm.Application.Interfaces;

public interface INotificarPedidoEditadoService
{
    Task NotificarAsync(Pedido pedido);
}
