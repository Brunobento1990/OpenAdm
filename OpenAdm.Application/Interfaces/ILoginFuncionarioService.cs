using OpenAdm.Application.Models;

namespace OpenAdm.Application.Interfaces;

public interface ILoginFuncionarioService
{
    Task<ResponseLoginFuncionarioViewModel> LoginFuncionarioAsync(RequestLogin requestLogin);
}
