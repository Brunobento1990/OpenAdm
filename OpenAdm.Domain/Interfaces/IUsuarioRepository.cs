using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetUsuarioByIdAsync(Guid id);
    Task<Usuario?> GetUsuarioByEmailAsync(string email);
}
