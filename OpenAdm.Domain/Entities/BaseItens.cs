
namespace OpenAdm.Domain.Entities;

public abstract class BaseItens : BaseEntity
{
    protected BaseItens(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, Guid produtoId) 
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ProdutoId = produtoId;
    }

    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; set; } = null!;
}
