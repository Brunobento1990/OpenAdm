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
        decimal? pesoReal, decimal? alturaReal, decimal? larguraReal, decimal? comprimentoReal)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
        PesoReal = pesoReal;
        AlturaReal = alturaReal;
        LarguraReal = larguraReal;
        ComprimentoReal = comprimentoReal;
    }

    public void Update(string descricao, decimal? pesoReal, decimal? alturaReal, decimal? larguraReal,
        decimal? comprimentoReal)
    {
        Descricao = descricao;
        PesoReal = pesoReal;
        AlturaReal = alturaReal;
        LarguraReal = larguraReal;
        ComprimentoReal = comprimentoReal;
    }

    public decimal? PesoReal { get; set; }
    public decimal? AlturaReal { get; set; }
    public decimal? LarguraReal { get; set; }
    public decimal? ComprimentoReal { get; set; }
    public string Descricao { get; private set; }
    public List<ItemPedido> ItensPedido { get; set; } = [];
    public List<Produto> Produtos { get; set; } = [];
}