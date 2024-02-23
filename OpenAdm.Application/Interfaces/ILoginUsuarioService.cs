using OpenAdm.Application.Models.Logins;

namespace OpenAdm.Application.Interfaces;

public interface ILoginUsuarioService
{
    Task<ResponseLoginUsuarioViewModel> LoginAsync(RequestLogin requestLogin);
}
