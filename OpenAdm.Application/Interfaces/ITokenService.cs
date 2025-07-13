using OpenAdm.Application.Models.Tokens;

namespace OpenAdm.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(object obj);
    string GenerateRefreshToken(Guid id);
    Task<TokenResponseGoogleModel> ValidarTokenGoogleAsync(string token);
    //UsuarioViewModel GetTokenUsuarioViewModel();
}
