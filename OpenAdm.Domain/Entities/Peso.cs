
namespace OpenAdm.Domain.Entities;

public sealed class Peso : BaseEntity
{
    public Peso(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string descricao)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
    }

    public string Descricao { get; private set; }
    public List<ItensPedido> ItensPedido { get; set; } = new();
    public List<Produto> Produtos { get; set; } = new();
}
