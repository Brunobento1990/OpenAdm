using OpenAdm.Application.Models.Representantes;

namespace OpenAdm.Application.Models.Logins;

public sealed class ResponseLoginRepresentanteViewModel(
    string token,
    RepresentanteViewModel representante)
{
    public string Token { get; set; } = token;
    public RepresentanteViewModel Usuario { get; set; } = representante;
}
