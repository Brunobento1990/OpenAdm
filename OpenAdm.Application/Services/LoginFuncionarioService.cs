using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Funcionarios;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class LoginFuncionarioService(
    ITokenService tokenService, 
    ILoginFuncionarioRepository loginFuncionarioRepository, 
    IParceiroAutenticado parceiroAutenticado)
    : ILoginFuncionarioService
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILoginFuncionarioRepository _loginFuncionarioRepository = loginFuncionarioRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado = parceiroAutenticado;

    public async Task<ResponseLoginFuncionarioViewModel> LoginFuncionarioAsync(RequestLogin requestLogin)
    {
        var funcionario = await _loginFuncionarioRepository.GetFuncionarioByEmailAsync(requestLogin.Email);

        if (funcionario == null || !PasswordAdapter.VerifyPassword(requestLogin.Senha, funcionario.Senha))
            throw new ExceptionApi("E-mail ou senha inválidos!");

        var funcionarioViewModel = new FuncionarioViewModel().ToModel(funcionario);
        var token = _tokenService.GenerateToken(funcionarioViewModel);

        return new(token, funcionarioViewModel);
    }
}
