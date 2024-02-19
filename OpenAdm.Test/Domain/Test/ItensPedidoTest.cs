using ExpectedObjects;
using Domain.Pkg.Entities;
using OpenAdm.Test.Domain.Builder;
using Domain.Pkg.Exceptions;

namespace OpenAdm.Test.Domain.Test;

public class ItensPedidoTest
{
    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    public void NaoDeveCriarItemPedidoSemQuantidade(int? quantidade)
    {
        Assert.Throws<ExceptionApi>(() => ItensPedidoBuilder.Init().SemQuantidade(quantidade).Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    public void NaoDeveCriarItemPedidoSemValorUnitario(int? valorUnitario)
    {
        Assert.Throws<ExceptionApi>(() => ItensPedidoBuilder.Init().SemValorUnitario(valorUnitario).Build());
    }

    [Fact]
    public void NaoDeveCriarItemPedidoSemPedidoId()
    {
        Assert.Throws<ExceptionApi>(() => ItensPedidoBuilder.Init().SemPedidoId(Guid.Empty).Build());
    }

    [Fact]
    public void NaoDeveCriarItemPedidoSemProdutoId()
    {
        Assert.Throws<ExceptionApi>(() => ItensPedidoBuilder.Init().SemProdutoId(Guid.Empty).Build());
    }

    [Fact]
    public void DeveCriarItemPedido()
    {
        var dto = ItensPedidoBuilder.Init().Build();

        var itemPedido = new ItensPedido(
            dto.Id,
            dto.DataDeCriacao,
            dto.DataDeAtualizacao,
            dto.Numero,
            dto.PesoId,
            dto.TamanhoId,
            dto.ProdutoId,
            dto.PedidoId,
            dto.ValorUnitario,
            dto.Quantidade);

        dto.ToExpectedObject().ShouldMatch(itemPedido);
    }
}
