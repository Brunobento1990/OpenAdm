using OpenAdm.Application.Models.Funcionarios;

namespace OpenAdm.Application.Models.Logins;

public class ResponseLoginFuncionarioViewModel(string token, FuncionarioViewModel userData)
{
    public string Token { get; set; } = token;
    public FuncionarioViewModel Usuario { get; set; } = userData;
}
