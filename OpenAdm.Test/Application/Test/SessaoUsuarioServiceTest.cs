using Microsoft.Extensions.Configuration;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class SessaoUsuarioServiceTest
{
    private readonly Mock<ISessaoUsuarioRepository> _sessaoUsuarioRepository;
    private readonly Mock<IParceiroAutenticado> _parceiroAutenticado;
    private readonly Mock<IUsuarioSessaoRequest> _usuarioSessaoRequest;
    private readonly SessaoUsuarioService _sessaoUsuarioService;

    public SessaoUsuarioServiceTest()
    {
        _sessaoUsuarioRepository = new Mock<ISessaoUsuarioRepository>();
        _parceiroAutenticado = new Mock<IParceiroAutenticado>();
        _usuarioSessaoRequest = new Mock<IUsuarioSessaoRequest>();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SessaoUsuario:ExpiracaoDias"] = "3"
            })
            .Build();

        _sessaoUsuarioService = new SessaoUsuarioService(
            _sessaoUsuarioRepository.Object,
            _parceiroAutenticado.Object,
            _usuarioSessaoRequest.Object,
            configuration);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task DeveCriarEPersistirNovaSessao(bool ehFuncionario)
    {
        var usuarioId = Guid.NewGuid();
        var parceiroId = Guid.NewGuid();
        _parceiroAutenticado.SetupGet(x => x.Id).Returns(parceiroId);

        var sessao = await _sessaoUsuarioService.CriarAsync(usuarioId, ehFuncionario);

        Assert.Equal(usuarioId, sessao.UsuarioId);
        Assert.Equal(parceiroId, sessao.ParceiroId);
        Assert.Equal(ehFuncionario, sessao.EhFuncionario);
        Assert.Equal(TimeSpan.FromDays(3), sessao.ExpiraEm - sessao.DataDeCriacao);
        _sessaoUsuarioRepository.Verify(x => x.AdicionarAsync(sessao), Times.Once);
        _sessaoUsuarioRepository.Verify(x => x.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeveDerrubarSomenteSessaoInformada()
    {
        var sessaoId = Guid.NewGuid();

        await _sessaoUsuarioService.DerrubarSessaoAsync(sessaoId);

        _sessaoUsuarioRepository.Verify(x => x.DerrubarSessaoAsync(sessaoId), Times.Once);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task DeveDerrubarSessoesPorUsuarioParceiroETipo(bool ehFuncionario)
    {
        var usuarioId = Guid.NewGuid();
        var parceiroId = Guid.NewGuid();
        _parceiroAutenticado.SetupGet(x => x.Id).Returns(parceiroId);

        await _sessaoUsuarioService.DerrubarSessoesAsync(usuarioId, ehFuncionario);

        _sessaoUsuarioRepository.Verify(
            x => x.DerrubarSessoesAsync(usuarioId, parceiroId, ehFuncionario), Times.Once);
    }
}
