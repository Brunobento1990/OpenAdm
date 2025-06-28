namespace OpenAdm.Domain.Entities;

public sealed class AcessoEcommerce
{
    public AcessoEcommerce(
        Guid id,
        DateTime dataDeCriacao,
        long quantidade,
        Guid parceiroId)
    {
        Id = id;
        DataDeCriacao = dataDeCriacao;
        Quantidade = quantidade;
        ParceiroId = parceiroId;
    }

    public Guid Id { get; private set; }
    public DateTime DataDeCriacao { get; private set; }
    public long Quantidade { get; private set; }
    public Guid ParceiroId { get; private set; }

    public void AdicionarAcesso()
    {
        Quantidade++;
    }

    public static AcessoEcommerce NovoAcesso(long quantidade, Guid parceiroId)
    {
        return new AcessoEcommerce(id: Guid.NewGuid(), dataDeCriacao: DateTime.Now, quantidade: quantidade, parceiroId);
    }
}
