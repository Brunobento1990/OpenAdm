using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITopUsuariosRepository
{
    Task<IList<TopUsuarios>> GetTopTresUsuariosToTalCompraAsync();
    Task<IList<TopUsuarios>> GetTopTresUsuariosToTalPedidosAsync();
}
