using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ILoginUsuarioRepository
{
    Task<Usuario?> LoginAsync(string email);
}
