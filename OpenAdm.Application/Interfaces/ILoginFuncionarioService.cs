using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Tokens;

namespace OpenAdm.Application.Interfaces;

public interface ILoginFuncionarioService
{
    Task<ResponseLoginFuncionarioViewModel> LoginFuncionarioAsync(RequestLogin requestLogin, ConfiguracaoDeToken configGenerateToken);
}
