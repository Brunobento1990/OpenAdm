using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Application.Models.Logins;

public class ResponseLoginUsuarioViewModel(UsuarioViewModel usuario, string token, string refreshToken)
{
    public UsuarioViewModel Usuario { get; set; } = usuario;
    public string Token { get; set; } = token;
    public string RefreshToken { get; set; } = refreshToken;
}
