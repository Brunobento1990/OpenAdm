using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Dtos.Representantes;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public sealed class LoginRepresentanteServiceTest
{
    private const string Senha = "senha-segura";

    private readonly Mock<ILoginRepresentanteRepository> _repository = new();
    private readonly Mock<ISessaoUsuarioService> _sessaoUsuarioService = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly LoginRepresentanteService _service;

    public LoginRepresentanteServiceTest()
    {
        _service = new LoginRepresentanteService(
            _repository.Object,
            _sessaoUsuarioService.Object,
            _tokenService.Object);
    }

    [Fact]
    public async Task DeveAutenticarRepresentanteEGerarToken()
    {
        var representante = CriarRepresentante();
        var sessao = SessaoUsuario.Criar(
            representante.Id,
            Guid.NewGuid(),
            TipoUsuario.Representante,
            10,
            Mock.Of<IUsuarioSessaoRequest>());
        _repository.Setup(x => x.ObterPorEmailAsync("representante@teste.com"))
            .ReturnsAsync(representante);
        _sessaoUsuarioService
            .Setup(x => x.CriarAsync(representante.Id, TipoUsuario.Representante))
            .ReturnsAsync(sessao);
        _tokenService.Setup(x => x.GenerateToken(sessao)).Returns("token-representante");

        var resultado = await _service.LoginAsync(new LoginRepresentanteDto
        {
            Email = " REPRESENTANTE@TESTE.COM ",
            Senha = Senha
        });

        Assert.Null(resultado.Error);
        Assert.Equal("token-representante", resultado.Result?.Token);
        Assert.Equal(representante.Id, resultado.Result?.Usuario.Id);
        _sessaoUsuarioService.Verify(
            x => x.CriarAsync(representante.Id, TipoUsuario.Representante), Times.Once);
    }

    [Fact]
    public async Task NaoDeveAutenticarRepresentanteInativo()
    {
        var representante = CriarRepresentante(inativo: true);
        _repository.Setup(x => x.ObterPorEmailAsync("representante@teste.com"))
            .ReturnsAsync(representante);

        var resultado = await _service.LoginAsync(new LoginRepresentanteDto
        {
            Email = "representante@teste.com",
            Senha = Senha
        });

        Assert.Equal("E-mail ou senha inválidos!", resultado.Error);
        _sessaoUsuarioService.Verify(
            x => x.CriarAsync(It.IsAny<Guid>(), It.IsAny<TipoUsuario>()), Times.Never);
    }

    [Fact]
    public async Task NaoDeveAutenticarComSenhaInvalida()
    {
        var representante = CriarRepresentante();
        _repository.Setup(x => x.ObterPorEmailAsync("representante@teste.com"))
            .ReturnsAsync(representante);

        var resultado = await _service.LoginAsync(new LoginRepresentanteDto
        {
            Email = "representante@teste.com",
            Senha = "senha-incorreta"
        });

        Assert.Equal("E-mail ou senha inválidos!", resultado.Error);
        _sessaoUsuarioService.Verify(
            x => x.CriarAsync(It.IsAny<Guid>(), It.IsAny<TipoUsuario>()), Times.Never);
    }

    private static Representante CriarRepresentante(bool inativo = false)
    {
        var builder = RepresentanteBuilder.Init()
            .ComSenha(PasswordAdapter.GenerateHash(Senha));

        if (inativo)
            builder.Inativo();

        return builder.Build();
    }
}
