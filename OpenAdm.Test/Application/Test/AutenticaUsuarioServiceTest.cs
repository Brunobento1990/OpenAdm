using OpenAdm.Application.Services;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public sealed class AutenticaUsuarioServiceTest
{
    private readonly Mock<ISessaoUsuarioRepository> _sessaoUsuarioRepository = new();
    private readonly Mock<IUsuarioAutenticado> _usuarioAutenticado = new();
    private readonly AutenticaUsuarioService _service;

    public AutenticaUsuarioServiceTest()
    {
        _usuarioAutenticado.SetupGet(x => x.TipoUsuario).Returns(TipoUsuario.Representante);
        _service = new AutenticaUsuarioService(
            _sessaoUsuarioRepository.Object,
            _usuarioAutenticado.Object);
    }

    [Fact]
    public async Task DeveValidarRepresentanteAtivo()
    {
        var representante = RepresentanteBuilder.Init().Build();
        _usuarioAutenticado.Setup(x => x.GetRepresentanteMiddlewareAsync())
            .ReturnsAsync(representante);

        var resultado = await _service.ValidarAsync();

        Assert.Null(resultado.Error);
        Assert.True(resultado.Result);
        _sessaoUsuarioRepository.Verify(
            x => x.DerrubarSessaoAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeveRevogarSessaoDeRepresentanteInativo()
    {
        var sessaoId = Guid.NewGuid();
        var representante = RepresentanteBuilder.Init().Inativo().Build();
        _usuarioAutenticado.SetupGet(x => x.SessaoId).Returns(sessaoId);
        _usuarioAutenticado.Setup(x => x.GetRepresentanteMiddlewareAsync())
            .ReturnsAsync(representante);

        var resultado = await _service.ValidarAsync();

        Assert.Equal("Seu acesso esta bloqueado!", resultado.Error);
        _sessaoUsuarioRepository.Verify(
            x => x.DerrubarSessaoAsync(sessaoId), Times.Once);
    }
}
