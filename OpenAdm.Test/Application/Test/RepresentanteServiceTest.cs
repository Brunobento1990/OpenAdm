using OpenAdm.Application.Dtos.Representantes;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public sealed class RepresentanteServiceTest
{
    private readonly Mock<IRepresentanteRepository> _repository;
    private readonly RepresentanteService _service;

    public RepresentanteServiceTest()
    {
        _repository = new Mock<IRepresentanteRepository>();
        _service = new RepresentanteService(_repository.Object);
    }

    [Fact]
    public async Task DeveCriarRepresentanteComDadosNormalizados()
    {
        var dto = new CriarRepresentanteDto
        {
            Nome = " Representante Teste ",
            Cpf = "529.982.247-25",
            Email = " REPRESENTANTE@TESTE.COM ",
            Telefone = " 11999999999 ",
            Senha = "senha-segura",
            ConfirmarSenha = "senha-segura"
        };
        _repository.Setup(x => x.AddAsync(It.IsAny<Representante>()))
            .ReturnsAsync((Representante representante) => representante);

        var resultado = await _service.CriarAsync(dto);

        Assert.Null(resultado.Error);
        Assert.Equal("Representante Teste", resultado.Result?.Nome);
        Assert.Equal("52998224725", resultado.Result?.Cpf);
        Assert.Equal("representante@teste.com", resultado.Result?.Email);
        _repository.Verify(x => x.AddAsync(It.Is<Representante>(r =>
            r.Senha != null && r.Senha != dto.Senha)), Times.Once);
    }

    [Theory]
    [InlineData("senha", null, "As senhas não conferem")]
    [InlineData(null, "senha", "As senhas não conferem")]
    [InlineData("senha", "outra-senha", "As senhas não conferem")]
    public async Task NaoDeveCriarQuandoAsSenhasForemInvalidas(
        string? senha, string? confirmarSenha, string erroEsperado)
    {
        var resultado = await _service.CriarAsync(new CriarRepresentanteDto
        {
            Nome = "Representante",
            Senha = senha,
            ConfirmarSenha = confirmarSenha
        });

        Assert.Equal(erroEsperado, resultado.Error);
        _repository.Verify(x => x.AddAsync(It.IsAny<Representante>()), Times.Never);
    }

    [Fact]
    public async Task NaoDeveCriarRepresentanteSemNome()
    {
        var resultado = await _service.CriarAsync(new CriarRepresentanteDto { Nome = " " });

        Assert.Equal("Informe o nome", resultado.Error);
        _repository.Verify(x => x.AddAsync(It.IsAny<Representante>()), Times.Never);
    }

    [Fact]
    public async Task NaoDeveCriarRepresentanteComCpfDuplicado()
    {
        _repository.Setup(x => x.ObterDuplicidadesAsync("52998224725", null, null))
            .ReturnsAsync((true, false));

        var resultado = await _service.CriarAsync(new CriarRepresentanteDto
        {
            Nome = "Representante",
            Cpf = "52998224725"
        });

        Assert.Equal("Já existe um representante cadastrado com este CPF", resultado.Error);
        _repository.Verify(x => x.AddAsync(It.IsAny<Representante>()), Times.Never);
    }

    [Fact]
    public async Task DeveInativarRepresentante()
    {
        var representante = RepresentanteBuilder.Init().Build();
        _repository.Setup(x => x.ObterPorIdAsync(representante.Id, true)).ReturnsAsync(representante);
        _repository.Setup(x => x.UpdateAsync(representante)).ReturnsAsync(representante);

        var resultado = await _service.InativarAtivarAsync(representante.Id, false);

        Assert.Null(resultado.Error);
        Assert.False(representante.Ativo);
        Assert.True(resultado.Result?.Resultado);
    }

    [Fact]
    public async Task DeveRetornarErroAoVisualizarRepresentanteInexistente()
    {
        var id = Guid.NewGuid();
        _repository.Setup(x => x.ObterPorIdAsync(id, false)).ReturnsAsync((Representante?)null);

        var resultado = await _service.VisualizarAsync(id);

        Assert.Equal("Não foi possível localizar o representante", resultado.Error);
    }
}
