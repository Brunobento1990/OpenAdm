using OpenAdm.Application.Models.Logins;

namespace OpenAdm.Application.Interfaces;

public interface ILoginUsuarioService
{
    Task<ResponseLoginUsuarioViewModel> LoginAsync(RequestLogin requestLogin);
    Task<ResponseLoginUsuarioViewModel> LoginV2Async(RequestLoginUsuario requestLogin);
}
