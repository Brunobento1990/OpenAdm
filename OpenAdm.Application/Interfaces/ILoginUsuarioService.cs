using OpenAdm.Application.Models.Logins;

namespace OpenAdm.Application.Interfaces;

public interface ILoginUsuarioService
{
    Task<ResponseLoginUsuarioViewModel> LoginGoogleAsync(RequestLoginGoogle requestLoginGoogle);
    Task<ResponseLoginUsuarioViewModel> LoginV2Async(RequestLoginUsuario requestLogin);
}
