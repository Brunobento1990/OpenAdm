using Domain.Pkg.Entities;
using ExpectedObjects;

namespace OpenAdm.Test.Domain.Test;

public class CategoriaTest
{
    [Fact]
    public void DeveCriarUmaCategoria()
    {
        var dto = new
        {
            Id = Guid.NewGuid(),
            DataDeCriacao = DateTime.Now,
            DataDeAtualizacao = DateTime.Now,
            Numero = 1,
            Descricao = "Teste 123"
        };

        var categoria = new Categoria(dto.Id, dto.DataDeCriacao, dto.DataDeAtualizacao, dto.Numero, dto.Descricao, null);

        Assert.NotNull(categoria);
        Assert.Equal(dto.Numero, categoria.Numero);
        Assert.Equal(dto.Id, categoria.Id);
        Assert.Equal(dto.Descricao, categoria.Descricao);
    }
}
