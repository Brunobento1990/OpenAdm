using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Funcionarios;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Application.Services;

public class LoginFuncionarioService
    : ILoginFuncionarioService
{
    private readonly ITokenService _tokenService;
    private readonly ILoginFuncionarioRepository _loginFuncionarioRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly ISessaoUsuarioRepository _sessaoUsuarioRepository;
    private readonly IConfiguration _configuration;

    public LoginFuncionarioService(ITokenService tokenService, ILoginFuncionarioRepository loginFuncionarioRepository,
        IParceiroAutenticado parceiroAutenticado,
        ISessaoUsuarioRepository sessaoUsuarioRepository,
        IConfiguration configuration)
    {
        _tokenService = tokenService;
        _loginFuncionarioRepository = loginFuncionarioRepository;
        _parceiroAutenticado = parceiroAutenticado;
        _sessaoUsuarioRepository = sessaoUsuarioRepository;
        _configuration = configuration;
    }

    public async Task<ResponseLoginFuncionarioViewModel> LoginFuncionarioAsync(RequestLogin requestLogin)
    {
        var funcionario =
            await _loginFuncionarioRepository.GetFuncionarioByEmailAsync(requestLogin.Email, _parceiroAutenticado.Id);

        if (funcionario == null || !PasswordAdapter.VerifyPassword(requestLogin.Senha, funcionario.Senha))
            throw new ExceptionApi("E-mail ou senha inválidos!");

        var funcionarioViewModel = new FuncionarioViewModel().ToModel(funcionario);
        var agora = DateTime.UtcNow;
        var dias = int.TryParse(_configuration["SessaoUsuario:ExpiracaoDias"], out var diasConfigurados)
            ? diasConfigurados
            : 10;
        var sessao = new SessaoUsuario(
            Guid.NewGuid(), agora, agora, funcionario.Id, _parceiroAutenticado.Id, true, null,
            agora.AddDays(dias), null, null, null, null, null, null);
        await _sessaoUsuarioRepository.AdicionarAsync(sessao);
        await _sessaoUsuarioRepository.SalvarAlteracoesAsync();

        var token = _tokenService.GenerateToken(sessao);
        return new(token, funcionarioViewModel);
    }
}
