using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IEnderecoEntregaPedidoRepository
{
    Task<EnderecoEntregaPedido?> GetEnderecoEntregaPedidoByPedidoIdAsync(Guid pedidoId);
}
