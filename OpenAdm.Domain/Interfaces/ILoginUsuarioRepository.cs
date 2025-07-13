using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ILoginUsuarioRepository
{
    Task<Usuario?> LoginAsync(string email);
    Task<Usuario?> LoginComGoogleAsync(string email);
}
