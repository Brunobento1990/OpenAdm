
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Entities;

public sealed class ProdutosMaisVendidos : BaseEntity
{
    public ProdutosMaisVendidos(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        decimal quantidadeProdutos,
        decimal quantidadePedidos,
        Guid produtoId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        QuantidadeProdutos = quantidadeProdutos;
        QuantidadePedidos = quantidadePedidos;
        ProdutoId = produtoId;
    }

    public decimal QuantidadeProdutos { get; private set; }
    public decimal QuantidadePedidos { get; private set; }
    public Guid ProdutoId { get; private set; }

    public void UpdateQuantidadeProdutos(decimal quantidadeProdutos)
    {
        if (quantidadeProdutos <= 0)
            throw new ExceptionApi("Quantidade de produtos inválida!");

        QuantidadeProdutos += quantidadeProdutos;
        QuantidadePedidos++;
    }
}
