using OpenAdm.Application.Models.Tokens;

namespace OpenAdm.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(Guid id, bool isFuncionario);
    string GenerateRefreshToken(Guid id, bool isFuncionario);
    Task<TokenResponseGoogleModel> ValidarTokenGoogleAsync(string token);
}
