using OpenAdm.Application.Models.Logins;

namespace OpenAdm.Application.Interfaces;

public interface ILoginFuncionarioService
{
    Task<ResponseLoginFuncionarioViewModel> LoginFuncionarioAsync(RequestLogin requestLogin);
}
