using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface ICreatePedidoService
{
    Task<PedidoViewModel> CreatePedidoAsync(IList<ItemPedidoModel> itensPedidoModels, Usuario usuario);
}
