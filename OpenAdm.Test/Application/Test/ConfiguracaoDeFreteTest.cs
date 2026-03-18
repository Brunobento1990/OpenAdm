using OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class ConfiguracaoDeFreteTest
{
    private readonly Mock<IConfiguracaoDeFreteRepository> _configuracaoDeFreteRepository;
    private readonly Mock<IParceiroAutenticado> _parceiroAutenticado;
    private readonly Mock<IUsuarioAutenticado> _usuarioAutenticado;
    private readonly ConfiguracaoDeFreteService _configuracaoDeFreteService;

    public ConfiguracaoDeFreteTest()
    {
        _configuracaoDeFreteRepository = new Mock<IConfiguracaoDeFreteRepository>();
        _parceiroAutenticado = new Mock<IParceiroAutenticado>();
        _usuarioAutenticado = new Mock<IUsuarioAutenticado>();
        _configuracaoDeFreteService = new ConfiguracaoDeFreteService(
            _configuracaoDeFreteRepository.Object,
            _parceiroAutenticado.Object,
            _usuarioAutenticado.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public async Task Nao_Deve_Criar_Config_Sem_Token(string? token)
    {
        var dto = new ConfiguracaoDeFreteDTO()
        {
            Token = token!
        };

        var resultado = await _configuracaoDeFreteService.CrairOuEditarAsync(dto);

        Assert.NotNull(resultado);
        Assert.False(resultado?.Sucesso);
        Assert.Equal("Informe o token", resultado?.Error);
    }
}