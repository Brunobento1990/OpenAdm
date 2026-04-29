using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Domain.Interfaces;

public interface IHomeRepository
{
    Task<IList<StatusPedidoHomeModel>> ObterStatusPedidosAsync();
    Task<int> CountDePedidosAsync();
    Task<IList<ContadorPedidoModel>> ContatorPedido7DiasAsync(DateTime dataInicio);
    Task<TotalizadorProtudoEstoqueHome?>  ObterTotalizadoProtudoEstoqueAsync();
}