namespace OpenAdm.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity(
        Guid id, 
        DateTime dataDeCriacao, 
        DateTime dataDeAtualizacao, 
        long numero)
    {
        Id = id;
        DataDeCriacao = dataDeCriacao;
        DataDeAtualizacao = dataDeAtualizacao;
        Numero = numero;
    }

    public Guid Id { get; protected set; }
    public DateTime DataDeCriacao { get; protected set; }
    public DateTime DataDeAtualizacao { get; protected set; }
    public long Numero { get; protected set; }
}
