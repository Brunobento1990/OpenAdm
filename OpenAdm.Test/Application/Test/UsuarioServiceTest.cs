using Moq;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public class UsuarioServiceTest
{
    private readonly Mock<IUsuarioRepository> _usuarioRepository = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<ISessaoUsuarioService> _sessaoUsuarioService = new();
    private readonly Mock<IUsuarioAutenticado> _usuarioAutenticado = new();
    private readonly UsuarioService _usuarioService;

    public UsuarioServiceTest()
    {
        _usuarioService = new UsuarioService(
            _usuarioRepository.Object,
            _tokenService.Object,
            Mock.Of<IPedidoRepository>(),
            _usuarioAutenticado.Object,
            Mock.Of<ICnpjConsultaService>(),
            _sessaoUsuarioService.Object);
    }

    [Fact]
    public async Task NaoDeveRecuperarSenhaComTokenExpirado()
    {
        var token = Guid.NewGuid();
        var usuario = UsuarioBuilder.Init()
            .ComRecuperacaoDeSenha(token, DateTime.UtcNow.AddHours(-2).AddMinutes(-1))
            .Build();
        ConfigurarUsuario(token, usuario);

        var exception = await Assert.ThrowsAsync<ExceptionApi>(() =>
            _usuarioService.RecuperarSenhaAsync(CriarDto(token)));

        Assert.Equal("Data de expiração de token expirada, recupere a senha novamente", exception.Message);
        _usuarioRepository.Verify(x => x.UpdateAsync(It.IsAny<OpenAdm.Domain.Entities.Usuario>()), Times.Never);
        _sessaoUsuarioService.Verify(x => x.CriarAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task DeveDerrubarSessoesAoTrocarSenha()
    {
        var usuario = UsuarioBuilder.Init().Build();
        _usuarioAutenticado.SetupGet(x => x.Id).Returns(usuario.Id);
        _usuarioRepository.Setup(x => x.GetUsuarioByIdAsync(usuario.Id)).ReturnsAsync(usuario);

        await _usuarioService.TrocarSenhaAsync(new UpdateSenhaUsuarioDto
        {
            Senha = "nova-senha-segura",
            ReSenha = "nova-senha-segura"
        });

        _usuarioRepository.Verify(x => x.UpdateAsync(usuario), Times.Once);
        _sessaoUsuarioService.Verify(x => x.DerrubarSessoesAsync(usuario.Id, false), Times.Once);
    }

    [Fact]
    public async Task DeveRecuperarSenhaComTokenValido()
    {
        var token = Guid.NewGuid();
        var usuario = UsuarioBuilder.Init()
            .ComRecuperacaoDeSenha(token, DateTime.UtcNow.AddMinutes(-119))
            .Build();
        var sessao = SessaoUsuario.Criar(
            usuario.Id, Guid.NewGuid(), false, 10, Mock.Of<IUsuarioSessaoRequest>());
        ConfigurarUsuario(token, usuario);
        _sessaoUsuarioService.Setup(x => x.CriarAsync(usuario.Id, false)).ReturnsAsync(sessao);
        _tokenService.Setup(x => x.GenerateToken(sessao)).Returns("novo-token");

        var resultado = await _usuarioService.RecuperarSenhaAsync(CriarDto(token));

        Assert.Equal("novo-token", resultado.Token);
        Assert.Null(usuario.TokenEsqueceuSenha);
        Assert.Null(usuario.DataExpiracaoTokenEsqueceuSenha);
        _usuarioRepository.Verify(x => x.UpdateAsync(usuario), Times.Once);
        _sessaoUsuarioService.Verify(x => x.DerrubarSessoesAsync(usuario.Id, false), Times.Once);
        _sessaoUsuarioService.Verify(x => x.CriarAsync(usuario.Id, false), Times.Once);
    }

    private void ConfigurarUsuario(Guid token, OpenAdm.Domain.Entities.Usuario usuario)
    {
        _usuarioRepository
            .Setup(x => x.GetUsuarioByTokenEsqueceuSenhaAsync(token))
            .ReturnsAsync(usuario);
    }

    private static RecuperarSenhaDto CriarDto(Guid token) => new()
    {
        TokenEsqueceuSenha = token,
        Senha = "nova-senha-segura",
        ReSenha = "nova-senha-segura"
    };
}
