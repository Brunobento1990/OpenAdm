using ExpectedObjects;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Test.Domain.Builder;

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
