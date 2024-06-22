using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(object obj);
    string GenerateRefreshToken(Guid id);
    UsuarioViewModel GetTokenUsuarioViewModel();
}
