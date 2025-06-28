using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Funcionarios;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class LoginFuncionarioService
    : ILoginFuncionarioService
{
    private readonly ITokenService _tokenService;
    private readonly ILoginFuncionarioRepository _loginFuncionarioRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;

    public LoginFuncionarioService(ITokenService tokenService, ILoginFuncionarioRepository loginFuncionarioRepository, IParceiroAutenticado parceiroAutenticado)
    {
        _tokenService = tokenService;
        _loginFuncionarioRepository = loginFuncionarioRepository;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<ResponseLoginFuncionarioViewModel> LoginFuncionarioAsync(RequestLogin requestLogin)
    {
        var funcionario = await _loginFuncionarioRepository.GetFuncionarioByEmailAsync(requestLogin.Email, _parceiroAutenticado.Id);

        if (funcionario == null || !PasswordAdapter.VerifyPassword(requestLogin.Senha, funcionario.Senha))
            throw new ExceptionApi("E-mail ou senha inválidos!");

        var funcionarioViewModel = new FuncionarioViewModel().ToModel(funcionario);
        var token = _tokenService.GenerateToken(funcionarioViewModel);

        return new(token, "", funcionarioViewModel);
    }
}
