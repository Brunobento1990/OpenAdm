using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface ICreatePedidoService
{
    Task<ResultPartner<CriarPedidoViewModel>> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto);
}
