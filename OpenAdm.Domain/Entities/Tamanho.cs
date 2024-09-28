using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Tamanho : BaseEntity
{
    public Tamanho(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string descricao)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
    }

    public void Update(string descricao)
    {
        Descricao = descricao;
    }

    public string Descricao { get; private set; }
    public List<ItemPedido> ItensPedido { get; set; } = [];
    public List<Produto> Produtos { get; set; } = [];
}
