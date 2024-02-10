namespace OpenAdm.Application.Models;

public class ResponseLoginFuncionarioViewModel(string token, FuncionarioViewModel userData)
{
    public string Token { get; set; } = token;
    public FuncionarioViewModel UserData { get; set; } = userData;
}
