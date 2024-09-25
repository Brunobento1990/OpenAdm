using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITopUsuariosRepository
{
    Task<IList<TopUsuarios>> GetTopTresUsuariosToTalCompraAsync();
    Task<IList<TopUsuarios>> GetTopTresUsuariosToTalPedidosAsync();
    Task AddAsync(TopUsuarios topUsuario);
    Task UpdateAsync(TopUsuarios topUsuario);
    Task<TopUsuarios?> GetByUsuarioIdAsync(Guid usuarioId);
}
