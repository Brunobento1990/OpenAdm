using OpenAdm.Application.Models.Funcionarios;

namespace OpenAdm.Application.Models.Logins;

public class ResponseLoginFuncionarioViewModel(string token, string refreshToken, FuncionarioViewModel userData)
{
    public string Token { get; set; } = token;
    public string RefreshToken { get; set; } = refreshToken;
    public FuncionarioViewModel Usuario { get; set; } = userData;
}
