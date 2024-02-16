using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Application.Models.Logins;

public class ResponseLoginUsuarioViewModel(UsuarioViewModel usuario, string token)
{
    public UsuarioViewModel Usuario { get; set; } = usuario;
    public string Token { get; set; } = token;
}
