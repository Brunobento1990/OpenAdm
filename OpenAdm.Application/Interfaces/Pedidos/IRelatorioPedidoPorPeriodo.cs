using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface IRelatorioPedidoPorPeriodo
{
    Task<RelatorioPedidoListagemDto> GetListagemAsync(RelatorioPedidoDto relatorioPedidoDto);
    Task<(byte[] pdf, int count)> GetRelatorioAsync(RelatorioPedidoDto relatorioPedidoDto);
}
