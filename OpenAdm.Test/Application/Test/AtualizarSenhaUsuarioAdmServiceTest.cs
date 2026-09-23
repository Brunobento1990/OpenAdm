using Moq;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public class AtualizarSenhaUsuarioAdmServiceTest
{
    private readonly Mock<IUsuarioRepository> _usuarioRepository = new();
    private readonly Mock<ISessaoUsuarioService> _sessaoUsuarioService = new();
    private readonly AtualizarSenhaUsuarioAdmService _service;

    public AtualizarSenhaUsuarioAdmServiceTest()
    {
        _service = new AtualizarSenhaUsuarioAdmService(
            _usuarioRepository.Object,
            _sessaoUsuarioService.Object);
    }

    [Fact]
    public async Task DeveDerrubarSessoesAoAtualizarSenha()
    {
        var usuario = UsuarioBuilder.Init().Build();
        _usuarioRepository.Setup(x => x.GetUsuarioByIdAsync(usuario.Id)).ReturnsAsync(usuario);

        var resultado = await _service.AtualizarAsync(new AtualizarSenhaUsuarioAdmDto
        {
            UsuarioId = usuario.Id,
            Senha = "nova-senha-segura",
            ConfirmarSenha = "nova-senha-segura"
        });

        Assert.True(resultado);
        _usuarioRepository.Verify(x => x.UpdateAsync(usuario), Times.Once);
        _sessaoUsuarioService.Verify(x => x.DerrubarSessoesAsync(usuario.Id, false), Times.Once);
    }
}
