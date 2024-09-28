namespace OpenAdm.Domain.Entities.Bases;

public abstract class BaseItem : BaseEntity
{
    protected BaseItem(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, Guid produtoId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ProdutoId = produtoId;
    }

    public Guid ProdutoId { get; protected set; }
    public Produto Produto { get; set; } = null!;
}
