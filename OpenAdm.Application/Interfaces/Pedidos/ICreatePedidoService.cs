using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Application.Dtos.Pedidos;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface ICreatePedidoService
{
    Task<PedidoViewModel> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto);
}
