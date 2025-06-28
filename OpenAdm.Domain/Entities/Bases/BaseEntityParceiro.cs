
namespace OpenAdm.Domain.Entities.Bases;

public abstract class BaseEntityParceiro : BaseEntity
{
    protected BaseEntityParceiro(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, Guid parceiroId) : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ParceiroId = parceiroId;
    }

    public Guid ParceiroId { get; protected set; }
}
