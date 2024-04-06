using Domain.Pkg.Entities;
using Domain.Pkg.Model;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Application.Interfaces.Pedidos;

public interface ICreatePedidoService
{
    Task<PedidoViewModel> CreatePedidoAsync(IList<ItensPedidoModel> itensPedidoModels, Usuario usuario);
}
