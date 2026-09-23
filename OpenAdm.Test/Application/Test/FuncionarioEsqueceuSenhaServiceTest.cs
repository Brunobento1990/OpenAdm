using Microsoft.Extensions.Configuration;
using OpenAdm.Application.Adapters;
using OpenAdm.Application.Dtos.Funcionarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public class FuncionarioEsqueceuSenhaServiceTest
{
    private readonly Mock<IFuncionarioEsqueceuSenhaRepository> _repository = new();
    private readonly Mock<IParceiroAutenticado> _parceiroAutenticado = new();
    private readonly Mock<ISessaoUsuarioService> _sessaoUsuarioService = new();
    private readonly FuncionarioEsqueceuSenhaService _service;

    public FuncionarioEsqueceuSenhaServiceTest()
    {
        _parceiroAutenticado.SetupGet(x => x.Id).Returns(Guid.NewGuid());
        _service = new FuncionarioEsqueceuSenhaService(
            Mock.Of<ILoginFuncionarioRepository>(),
            _repository.Object,
            Mock.Of<IEmailApiService>(),
            _parceiroAutenticado.Object,
            Mock.Of<IConfiguration>(),
            _sessaoUsuarioService.Object);
    }

    [Fact]
    public async Task DeveRecuperarSenhaDoFuncionario()
    {
        var senhaAnterior = PasswordAdapter.GenerateHash("senha-anterior");
        var funcionario = FuncionarioBuilder.Init().ComSenha(senhaAnterior).Build();
        var solicitacao = FuncionarioEsqueceuSenhaBuilder.Init()
            .ComFuncionario(funcionario)
            .ComExpiracao(DateTime.UtcNow.AddHours(1))
            .Build();
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
        _sessaoUsuarioService.Verify(
            x => x.DerrubarSessoesAsync(solicitacao.Funcionario.Id, true), Times.Once);
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
        var solicitacao = FuncionarioEsqueceuSenhaBuilder.Init()
            .ComExpiracao(DateTime.UtcNow.AddHours(1))
            .Resetado()
            .Build();
        ConfigurarSolicitacao(solicitacao);

        var resultado = await _service.RecuperarSenhaAsync(CriarDto(solicitacao.Token));

        Assert.Equal("Este token de recuperação já foi utilizado!", resultado.Error);
        VerificarQueNaoSalvou();
    }

    [Fact]
    public async Task NaoDeveRecuperarSenhaComTokenExpirado()
    {
        var solicitacao = FuncionarioEsqueceuSenhaBuilder.Init()
            .ComExpiracao(DateTime.UtcNow.AddMinutes(-1))
            .Build();
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

}
