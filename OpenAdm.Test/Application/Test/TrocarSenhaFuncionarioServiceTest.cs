using OpenAdm.Application.Adapters;
using OpenAdm.Application.Dtos.Funcionarios;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class TrocarSenhaFuncionarioServiceTest
{
    private readonly Mock<IFuncionarioRepository> _funcionarioRepository = new();
    private readonly Mock<IUsuarioAutenticado> _usuarioAutenticado = new();
    private readonly Mock<IParceiroAutenticado> _parceiroAutenticado = new();
    private readonly TrocarSenhaFuncionarioService _service;

    public TrocarSenhaFuncionarioServiceTest()
    {
        _usuarioAutenticado.SetupGet(x => x.Id).Returns(Guid.NewGuid());
        _parceiroAutenticado.SetupGet(x => x.Id).Returns(Guid.NewGuid());
        _service = new TrocarSenhaFuncionarioService(
            _funcionarioRepository.Object,
            _usuarioAutenticado.Object,
            _parceiroAutenticado.Object);
    }

    [Fact]
    public async Task DeveTrocarSenhaQuandoSenhaAtualForValida()
    {
        var funcionario = CriarFuncionario("senha-atual");
        ConfigurarFuncionario(funcionario);

        var resultado = await _service.TrocarAsync(CriarDto("senha-atual"));

        Assert.Null(resultado.Error);
        Assert.True(resultado.Result?.Resultado);
        Assert.True(PasswordAdapter.VerifyPassword("nova-senha", funcionario.Senha));
        _funcionarioRepository.Verify(x => x.Update(funcionario), Times.Once);
        _funcionarioRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task NaoDeveTrocarSenhaQuandoSenhaAtualForInvalida()
    {
        var funcionario = CriarFuncionario("senha-atual");
        ConfigurarFuncionario(funcionario);

        var resultado = await _service.TrocarAsync(CriarDto("senha-incorreta"));

        Assert.Equal("Senha atual inválida!", resultado.Error);
        _funcionarioRepository.Verify(x => x.Update(It.IsAny<Funcionario>()), Times.Never);
        _funcionarioRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task NaoDeveTrocarSenhaQuandoConfirmacaoForDiferente()
    {
        var dto = CriarDto("senha-atual");
        dto.ConfirmacaoSenha = "outra-senha";

        var resultado = await _service.TrocarAsync(dto);

        Assert.Equal("As senhas não conferem!", resultado.Error);
        _funcionarioRepository.Verify(
            x => x.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _funcionarioRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task NaoDeveTrocarSenhaQuandoNovaSenhaForIgualAAtual()
    {
        var funcionario = CriarFuncionario("senha-atual");
        ConfigurarFuncionario(funcionario);
        var dto = new TrocarSenhaFuncionarioDto
        {
            SenhaAtual = "senha-atual",
            Senha = "senha-atual",
            ConfirmacaoSenha = "senha-atual"
        };

        var resultado = await _service.TrocarAsync(dto);

        Assert.Equal("A nova senha deve ser diferente da senha atual!", resultado.Error);
        _funcionarioRepository.Verify(x => x.Update(It.IsAny<Funcionario>()), Times.Never);
        _funcionarioRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    private void ConfigurarFuncionario(Funcionario funcionario)
    {
        _funcionarioRepository
            .Setup(x => x.ObterPorIdAsync(_usuarioAutenticado.Object.Id, _parceiroAutenticado.Object.Id))
            .ReturnsAsync(funcionario);
    }

    private Funcionario CriarFuncionario(string senha) => new(
        _usuarioAutenticado.Object.Id,
        DateTime.UtcNow,
        DateTime.UtcNow,
        1,
        "funcionario@email.com",
        PasswordAdapter.GenerateHash(senha),
        "Funcionário",
        null,
        null,
        true,
        _parceiroAutenticado.Object.Id);

    private static TrocarSenhaFuncionarioDto CriarDto(string senhaAtual) => new()
    {
        SenhaAtual = senhaAtual,
        Senha = "nova-senha",
        ConfirmacaoSenha = "nova-senha"
    };
}
