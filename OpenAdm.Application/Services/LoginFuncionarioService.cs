﻿using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Funcionarios;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using static BCrypt.Net.BCrypt;

namespace OpenAdm.Application.Services;

public class LoginFuncionarioService(ITokenService tokenService, ILoginFuncionarioRepository loginFuncionarioRepository)
    : ILoginFuncionarioService
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILoginFuncionarioRepository _loginFuncionarioRepository = loginFuncionarioRepository;

    public async Task<ResponseLoginFuncionarioViewModel> LoginFuncionarioAsync(RequestLogin requestLogin, ConfiguracaoDeToken configGenerateToken)
    {
        var funcionario = await _loginFuncionarioRepository.GetFuncionarioByEmailAsync(requestLogin.Email);

        if (funcionario == null || !Verify(requestLogin.Senha, funcionario.Senha))
            throw new ExceptionApi(CodigoErrors.ErrorEmailOuSenhaInvalido);

        var funcionarioViewModel = new FuncionarioViewModel().ToModel(funcionario);
        var token = _tokenService.GenerateToken(funcionarioViewModel, configGenerateToken);

        return new(token, funcionarioViewModel);
    }
}
