using System.ComponentModel.DataAnnotations;
using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities;

public sealed class Estoque : BaseEntity
{
    public Estoque(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, Guid produtoId,
        decimal quantidade, Guid? tamanhoId, Guid? pesoId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ProdutoId = produtoId;
        Quantidade = quantidade;
        TamanhoId = tamanhoId;
        PesoId = pesoId;
    }

    public void AplicarMovimentacao(decimal quantidade, TipoMovimentacaoDeProduto tipo, bool permitirEstoqueNegativo)
    {
        var novaQuantidade = Quantidade;

        if (tipo == TipoMovimentacaoDeProduto.Entrada)
        {
            novaQuantidade += quantidade;
        }
        else
        {
            novaQuantidade -= quantidade;
        }

        if (!permitirEstoqueNegativo && novaQuantidade < 0)
        {
            throw new ValidationException("Estoque negativo não permitido");
        }

        Quantidade = novaQuantidade;
        DataDeAtualizacao = DateTime.Now;
    }

    public void AjustarQuantidade(decimal quantidade, bool permitirEstoqueNegativo)
    {
        if (!permitirEstoqueNegativo && quantidade < 0)
        {
            throw new ValidationException("Estoque negativo não permitido");
        }

        Quantidade = quantidade;
        DataDeAtualizacao = DateTime.Now;
    }

    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; set; } = null!;
    public Guid? TamanhoId { get; private set; }
    public Tamanho? Tamanho { get; set; }
    public Guid? PesoId { get; private set; }
    public Peso? Peso { get; set; }
    public decimal Quantidade { get; private set; }

    public static Estoque NovoEstoque(
        decimal quantidade,
        Guid produtoId,
        Guid? tamanhoId,
        Guid? pesoId)
    {
        return new Estoque(id: Guid.NewGuid(), dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now, numero: 0, produtoId: produtoId, quantidade: quantidade,
            tamanhoId: tamanhoId, pesoId: pesoId);
    }
}