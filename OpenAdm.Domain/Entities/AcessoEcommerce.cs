namespace OpenAdm.Domain.Entities;

public sealed class AcessoEcommerce
{
    public AcessoEcommerce(
        Guid id,
        DateTime dataDeCriacao,
        long quantidade)
    {
        Id = id;
        DataDeCriacao = dataDeCriacao;
        Quantidade = quantidade;
    }

    public Guid Id { get; private set; }
    public DateTime DataDeCriacao { get; private set; }
    public long Quantidade { get; private set; }

    public void AdicionarAcesso()
    {
        Quantidade++;
    }

    public static AcessoEcommerce NovoAcesso(long quantidade)
    {
        return new AcessoEcommerce(id: Guid.NewGuid(), dataDeCriacao: DateTime.Now, quantidade: quantidade);
    }
}
