using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITopUsuariosRepository
{
    Task<IList<TopUsuario>> GetTopTresUsuariosToTalCompraAsync();
    Task<IList<TopUsuario>> GetTopTresUsuariosToTalPedidosAsync();
    Task AddAsync(TopUsuario topUsuario);
    Task UpdateAsync(TopUsuario topUsuario);
    Task<TopUsuario?> GetByUsuarioIdAsync(Guid usuarioId);
}
