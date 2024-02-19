using ExpectedObjects;
using Domain.Pkg.Entities;
using OpenAdm.Test.Domain.Builder;
using Domain.Pkg.Exceptions;

namespace OpenAdm.Test.Domain.Test;

public class TabelaDePrecoTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarTabelaDePrecoSemDescricao(string descricao)
    {
        Assert.Throws<ExceptionApi>(() => TabelaDePrecoBuilder.Init().SemDescricao(descricao).Build());
    }

    [Fact]
    public void DeveCriarTabelaDePreco()
    {
        var dto = TabelaDePrecoBuilder.Init().Build();

        var tebaleDePreco = new TabelaDePreco(dto.Id, dto.DataDeCriacao, dto.DataDeAtualizacao, dto.Numero, dto.Descricao, dto.AtivaEcommerce);

        dto.ToExpectedObject().ShouldMatch(tebaleDePreco);
    }
}
