namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IDeletePedidoService
{
    Task<bool> DeletePedidoAsync(Guid id);
}
