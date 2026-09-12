using Microsoft.Extensions.Configuration;
using OpenAdm.Application.Adapters;
using OpenAdm.Application.Dtos.Funcionarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class FuncionarioEsqueceuSenhaServiceTest
{
    private readonly Mock<IFuncionarioEsqueceuSenhaRepository> _repository = new();
    private readonly Mock<IParceiroAutenticado> _parceiroAutenticado = new();
    private readonly FuncionarioEsqueceuSenhaService _service;

    public FuncionarioEsqueceuSenhaServiceTest()
    {
        _parceiroAutenticado.SetupGet(x => x.Id).Returns(Guid.NewGuid());
        _service = new FuncionarioEsqueceuSenhaService(
            Mock.Of<ILoginFuncionarioRepository>(),
            _repository.Object,
            Mock.Of<IEmailApiService>(),
            _parceiroAutenticado.Object,
            Mock.Of<IConfiguration>());
    }

    [Fact]
    public async Task DeveRecuperarSenhaDoFuncionario()
    {
        var senhaAnterior = PasswordAdapter.GenerateHash("senha-anterior");
        var solicitacao = CriarSolicitacao(DateTime.UtcNow.AddHours(1), false, senhaAnterior);
        _repository
            .Setup(x => x.ObterPorTokenAsync(solicitacao.Token, _parceiroAutenticado.Object.Id))
            .ReturnsAsync(solicitacao);

        var resultado = await _service.RecuperarSenhaAsync(new RecuperarSenhaFuncionarioDto
        {
            Token = solicitacao.Token,
            Senha = "nova-senha",
            ConfirmacaoSenha = "nova-senha"
        });

        Assert.Null(resultado.Error);
        Assert.True(resultado.Result?.Resultado);
        Assert.True(solicitacao.Resetado);
        Assert.True(PasswordAdapter.VerifyPassword("nova-senha", solicitacao.Funcionario.Senha));
        _repository.Verify(x => x.Update(solicitacao), Times.Once);
        _repository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task NaoDeveRecuperarSenhaComTokenInexistente()
    {
        var token = Guid.NewGuid();
        _repository
            .Setup(x => x.ObterPorTokenAsync(token, _parceiroAutenticado.Object.Id))
            .ReturnsAsync((FuncionarioEsqueceuSenha?)null);

        var resultado = await _service.RecuperarSenhaAsync(CriarDto(token));

        Assert.Equal("Token de recuperação inválido!", resultado.Error);
        VerificarQueNaoSalvou();
    }

    [Fact]
    public async Task NaoDeveRecuperarSenhaComTokenJaUtilizado()
    {
        var solicitacao = CriarSolicitacao(DateTime.UtcNow.AddHours(1), true);
        ConfigurarSolicitacao(solicitacao);

        var resultado = await _service.RecuperarSenhaAsync(CriarDto(solicitacao.Token));

        Assert.Equal("Este token de recuperação já foi utilizado!", resultado.Error);
        VerificarQueNaoSalvou();
    }

    [Fact]
    public async Task NaoDeveRecuperarSenhaComTokenExpirado()
    {
        var solicitacao = CriarSolicitacao(DateTime.UtcNow.AddMinutes(-1));
        ConfigurarSolicitacao(solicitacao);

        var resultado = await _service.RecuperarSenhaAsync(CriarDto(solicitacao.Token));

        Assert.Equal("Token expirado, solicite uma nova recuperação de senha!", resultado.Error);
        VerificarQueNaoSalvou();
    }

    private void ConfigurarSolicitacao(FuncionarioEsqueceuSenha solicitacao)
    {
        _repository
            .Setup(x => x.ObterPorTokenAsync(solicitacao.Token, _parceiroAutenticado.Object.Id))
            .ReturnsAsync(solicitacao);
    }

    private void VerificarQueNaoSalvou()
    {
        _repository.Verify(x => x.Update(It.IsAny<FuncionarioEsqueceuSenha>()), Times.Never);
        _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    private static RecuperarSenhaFuncionarioDto CriarDto(Guid token) => new()
    {
        Token = token,
        Senha = "nova-senha",
        ConfirmacaoSenha = "nova-senha"
    };

    private static FuncionarioEsqueceuSenha CriarSolicitacao(
        DateTime expiracao,
        bool resetado = false,
        string? senha = null)
    {
        var parceiroId = Guid.NewGuid();
        var funcionario = new Funcionario(
            Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 1, "funcionario@email.com",
            senha ?? PasswordAdapter.GenerateHash("senha-anterior"), "Funcionário", null, null, true, parceiroId);
        var solicitacao = new FuncionarioEsqueceuSenha(
            Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 1, funcionario.Id,
            Guid.NewGuid(), expiracao, resetado, parceiroId);

        typeof(FuncionarioEsqueceuSenha)
            .GetProperty(nameof(FuncionarioEsqueceuSenha.Funcionario))!
            .SetValue(solicitacao, funcionario);

        return solicitacao;
    }
}
