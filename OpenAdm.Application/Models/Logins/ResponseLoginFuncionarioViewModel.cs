using OpenAdm.Application.Models.Funcionarios;

namespace OpenAdm.Application.Models.Logins;

public class ResponseLoginFuncionarioViewModel(string token, FuncionarioViewModel userData, Guid xApi)
{
    public string Token { get; set; } = token;
    public FuncionarioViewModel UserData { get; set; } = userData;
    public Guid XApi { get; set; } = xApi;
}
