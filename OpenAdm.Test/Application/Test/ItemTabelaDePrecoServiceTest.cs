using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class ItemTabelaDePrecoServiceTest
{
    private readonly Mock<IItemTabelaDePrecoRepository> _repository = new();

    [Fact]
    public async Task CreateItemTabelaDePrecoAsync_DeveRetornarItemCompleto()
    {
        var produto = CriarProduto();
        var peso = new Peso(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 1, "1 kg", 1, null, null, null, true);
        var tamanho = new Tamanho(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 1, "M", null, null, null, null, true);
        var itemCompleto = new ItemTabelaDePreco(
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow,
            1,
            produto.Id,
            10,
            20,
            Guid.NewGuid(),
            tamanho.Id,
            peso.Id)
        {
            Produto = produto,
            Peso = peso,
            Tamanho = tamanho
        };
        var dto = new CreateItensTabelaDePrecoDto
        {
            ProdutoId = produto.Id,
            TabelaDePrecoId = itemCompleto.TabelaDePrecoId,
            PesoId = peso.Id,
            TamanhoId = tamanho.Id,
            ValorUnitarioAtacado = 10,
            ValorUnitarioVarejo = 20
        };
        _repository
            .Setup(x => x.GetItemTabelaDePrecoByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(itemCompleto);

        var resultado = await CriarService().CreateItemTabelaDePrecoAsync(dto);

        Assert.Equal(produto.Id, resultado.Produto.Id);
        Assert.Equal(peso.Id, resultado.Peso?.Id);
        Assert.Equal(tamanho.Id, resultado.Tamanho?.Id);
        _repository.Verify(x => x.AddAsync(It.IsAny<ItemTabelaDePreco>()), Times.Once);
    }

    [Fact]
    public async Task UpdateValoresAsync_DeveEditarValoresDoItem()
    {
        var itemTabela = new ItemTabelaDePreco(
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow,
            1,
            Guid.NewGuid(),
            10,
            20,
            Guid.NewGuid(),
            null,
            null);
        var dto = new UpdateItemTabelaDePrecoDto
        {
            Id = itemTabela.Id,
            ValorUnitarioAtacado = 30,
            ValorUnitarioVarejo = 40
        };
        _repository
            .Setup(x => x.GetItemTabelaDePrecoByIdAsync(dto.Id))
            .ReturnsAsync(itemTabela);

        await CriarService().UpdateValoresAsync(dto);

        Assert.Equal(30, itemTabela.ValorUnitarioAtacado);
        Assert.Equal(40, itemTabela.ValorUnitarioVarejo);
        _repository.Verify(x => x.UpdateAsync(itemTabela), Times.Once);
    }

    [Fact]
    public async Task UpdateValoresAsync_DeveRetornarErroQuandoItemNaoExistir()
    {
        var dto = new UpdateItemTabelaDePrecoDto
        {
            Id = Guid.NewGuid(),
            ValorUnitarioAtacado = 1,
            ValorUnitarioVarejo = 1
        };

        var exception = await Assert.ThrowsAsync<ExceptionApi>(() => CriarService().UpdateValoresAsync(dto));

        Assert.Equal("Não foi possível localizar o item da tabela de preço!", exception.Message);
        _repository.Verify(x => x.UpdateAsync(It.IsAny<ItemTabelaDePreco>()), Times.Never);
    }

    private ItemTabelaDePrecoService CriarService() => new(_repository.Object);

    private static Produto CriarProduto() => new(
        Guid.NewGuid(),
        DateTime.UtcNow,
        DateTime.UtcNow,
        1,
        "Produto",
        null,
        Guid.NewGuid(),
        null,
        null,
        null,
        false,
        false,
        true);
}
