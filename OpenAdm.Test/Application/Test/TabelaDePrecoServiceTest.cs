using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Application.Test;

public class TabelaDePrecoServiceTest
{
    [Fact]
    public async Task DeveCriarTabelaDePrecoERetornarModel()
    {
        var pesoId = Guid.NewGuid();
        var tamanhoId = Guid.NewGuid();
        var date = DateTime.Now;

        var pesos = new List<Peso>()
        {
            new Peso(pesoId, date, date, 0, "Peso", 0)
        };

        var tamanhos = new List<Tamanho>()
        {
            new Tamanho(tamanhoId, date, date, 0, "Tamanho", 0)
        };

        var tabelaDePrecoDto = new CreateTabelaDePrecoDto()
        {
            Descricao = "Teste service",
            ItensTabelaDePreco = new List<CreateItensTabelaDePrecoDto>()
            {
                new()
                {
                    PesoId = pesoId,
                    ProdutoId = Guid.NewGuid(),
                    ValorUnitarioAtacado = 5,
                    ValorUnitarioVarejo = 2
                },
                new()
                {
                    ProdutoId = Guid.NewGuid(),
                    TamanhoId = tamanhoId,
                    ValorUnitarioAtacado = 15,
                    ValorUnitarioVarejo = 2
                }
            }
        };

        var tamanhosIds = tamanhos.Select(x => x.Id).ToList();
        var pesosIds = pesos.Select(x => x.Id).ToList();

        var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
        var tamanhosRepositoryMock = new Mock<ITamanhoRepository>();
        var pesosRepositoryMock = new Mock<IPesoRepository>();

        tamanhosRepositoryMock
            .Setup((x) =>
                x.GetTamanhosByIdsAsync(tamanhosIds))
            .ReturnsAsync(tamanhos);

        pesosRepositoryMock
            .Setup((x) =>
                x.GetPesosByIdsAsync(pesosIds))
            .ReturnsAsync(pesos);

        var service = new TabelaDePrecoService(tabelaDePrecoRepositoryMock.Object, tamanhosRepositoryMock.Object, pesosRepositoryMock.Object);

        var tabelaDePrecoViewModel = await service.CreateTabelaDePrecoAsync(tabelaDePrecoDto);

        Assert.NotNull(tabelaDePrecoViewModel);
        Assert.Equal(tabelaDePrecoDto.Descricao, tabelaDePrecoViewModel.Descricao);
        Assert.Equal(tabelaDePrecoDto.ItensTabelaDePreco.Count, tabelaDePrecoViewModel.ItensTabelaDePreco.Count);
    }

    [Fact]
    public async Task NaoDeveExcluirUnicaTabelaDePreco()
    {
        var tabelaDePreco = TabelaDePrecoBuilder.Init().Build();

        var tabelaDePrecoRepositoryMock = new Mock<ITabelaDePrecoRepository>();
        var tamanhosRepositoryMock = new Mock<ITamanhoRepository>();
        var pesosRepositoryMock = new Mock<IPesoRepository>();

        tabelaDePrecoRepositoryMock.Setup(
            (x) => x.GetTabelaDePrecoByIdAsync(tabelaDePreco.Id))
            .ReturnsAsync(tabelaDePreco);

        var service = new TabelaDePrecoService(tabelaDePrecoRepositoryMock.Object, tamanhosRepositoryMock.Object, pesosRepositoryMock.Object);

        await Assert.ThrowsAsync<ExceptionApi>(
            async () => await service.DeleteTabelaDePrecoAsync(tabelaDePreco.Id));
    }
}
