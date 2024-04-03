using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IRelatorioPedidoPorPeriodo
{
    Task<(byte[] pdf, int count)> GetRelatorioAsync(RelatorioPedidoDto relatorioPedidoDto);
}
