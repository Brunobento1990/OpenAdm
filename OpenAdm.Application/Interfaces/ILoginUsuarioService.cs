using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Tokens;

namespace OpenAdm.Application.Interfaces;

public interface ILoginUsuarioService
{
    Task<ResponseLoginUsuarioViewModel> LoginAsync(RequestLogin requestLogin, ConfiguracaoDeToken configuracaoDeToken);
}
