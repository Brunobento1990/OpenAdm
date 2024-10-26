using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IEnderecoEntregaPedidoRepository
{
    Task AddAsync(EnderecoEntregaPedido enderecoEntregaPedido);
    Task<EnderecoEntregaPedido?> GetByPedidoIdAsync(Guid pedidoId);
}
