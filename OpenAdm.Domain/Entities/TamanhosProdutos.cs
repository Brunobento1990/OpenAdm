namespace OpenAdm.Domain.Entities;

public sealed class TamanhosProdutos
{
    public TamanhosProdutos(Guid id, Guid produtoId, Guid tamanhoId)
    {
        Id = id;
        ProdutoId = produtoId;
        TamanhoId = tamanhoId;
    }

    public Guid Id { get; private set; }
    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; set; } = null!;
    public Guid TamanhoId { get; private set; }
    public Tamanho Tamanho { get; set; } = null!;
}
