using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ITopUsuariosRepository
{
    Task<IList<TopUsuario>> GetTopTresUsuariosToTalCompraAsync(Guid parceiroId);
    Task<IList<TopUsuario>> GetTopTresUsuariosToTalPedidosAsync(Guid parceiroId);
    Task AddAsync(TopUsuario topUsuario);
    Task UpdateAsync(TopUsuario topUsuario);
    Task<TopUsuario?> GetByUsuarioIdAsync(Guid usuarioId, Guid parceiroId);
}
