using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Domain.Test;

public class ProdutoTest
{
    [Fact]
    public void DeveCriarProduto()
    {
        var dto = ProdutoBuilder.Init().Build();

        var produto = new Produto(dto.Id, dto.DataDeCriacao, dto.DataDeAtualizacao, dto.Numero, dto.Descricao, dto.EspecificacaoTecnica, dto.Foto, dto.CategoriaId, dto.Referencia);

        Assert.NotNull(produto);
        Assert.Equal(dto.Id, produto.Id);   
        Assert.Equal(dto.DataDeCriacao, produto.DataDeCriacao);   
        Assert.Equal(dto.DataDeAtualizacao, produto.DataDeAtualizacao);   
        Assert.Equal(dto.Numero, produto.Numero);   
        Assert.Equal(dto.Descricao, produto.Descricao);   
        Assert.Equal(dto.EspecificacaoTecnica, produto.EspecificacaoTecnica);   
        Assert.Equal(dto.Foto, produto.Foto);   
        Assert.Equal(dto.CategoriaId, produto.CategoriaId);   
        Assert.Equal(dto.Referencia, produto.Referencia);   
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarProdutoSemDescricao(string descricao)
    {
        Assert.Throws<ExceptionApi>(() => ProdutoBuilder.Init().SemDescricao(descricao).Build());   
    }

    [Fact]
    public void NaoDeveCriarProdutoComEspecificacaoTecnicaInvalida()
    {
        Assert.Throws<ExceptionApi>(() => ProdutoBuilder.Init().EspecificacaoTecnicaoInvalida().Build());   
    }

    [Fact]
    public void NaoDeveCriarProdutoComReferenciaInvalida()
    {
        Assert.Throws<ExceptionApi>(() => ProdutoBuilder.Init().ReferenciaInvalida().Build());
    }
}
