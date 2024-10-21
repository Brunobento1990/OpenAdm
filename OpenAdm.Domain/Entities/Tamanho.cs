using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Tamanho : BaseEntity
{
    public Tamanho(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string descricao,
        decimal? pesoReal)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
        PesoReal = pesoReal;
    }

    public void Update(string descricao, decimal? pesoReal)
    {
        Descricao = descricao;
        PesoReal = pesoReal;
    }
    public decimal? PesoReal { get; set; }
    public string Descricao { get; private set; }
    public List<ItemPedido> ItensPedido { get; set; } = [];
    public List<Produto> Produtos { get; set; } = [];
}
