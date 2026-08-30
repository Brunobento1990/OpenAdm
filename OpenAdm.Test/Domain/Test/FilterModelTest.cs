using System.Linq.Expressions;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Model;

namespace OpenAdm.Test.Domain.Test;

public class FilterModelTest
{
    [Theory]
    [InlineData(-1, 10, "Numero")]
    [InlineData(0, 0, "Numero")]
    [InlineData(0, 101, "Numero")]
    [InlineData(0, 10, "")]
    [InlineData(0, 10, "PropriedadeInexistente")]
    public void DeveRejeitarParametrosDePaginacaoInvalidos(int skip, int take, string orderBy)
    {
        var filter = new CategoriaFilter
        {
            Skip = skip,
            Take = take,
            OrderBy = orderBy
        };

        Assert.Throws<ExceptionApi>(() => filter.ValidarEObterPropriedadeDeOrdenacao());
    }

    [Fact]
    public void DeveNormalizarNomeDaPropriedadeDeOrdenacao()
    {
        var filter = new CategoriaFilter { OrderBy = "descricao" };

        var propriedade = filter.ValidarEObterPropriedadeDeOrdenacao();

        Assert.Equal(nameof(Categoria.Descricao), propriedade);
    }

    private sealed class CategoriaFilter : FilterModel<Categoria>
    {
        public override Expression<Func<Categoria, bool>>? GetWhereBySearch() => null;
    }
}
