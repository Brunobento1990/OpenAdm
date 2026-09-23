using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(OpenAdm.Domain.Entities.OpenAdm.SessaoUsuario sessao);
    Task<TokenResponseGoogleModel> ValidarTokenGoogleAsync(string token);
    ResultPartner<ValidaTokenModel> ValidarToken(string token, bool validaLifeTime = true);
}
