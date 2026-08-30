using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Peso : BaseEntity
{
    public Peso(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string descricao,
        decimal? pesoReal, decimal? alturaReal, decimal? larguraReal, decimal? comprimentoReal, bool ativo)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
        PesoReal = pesoReal;
        AlturaReal = alturaReal;
        LarguraReal = larguraReal;
        ComprimentoReal = comprimentoReal;
        Ativo = ativo;
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

    public string Descricao { get; private set; }
    public decimal? PesoReal { get; private set; }
    public decimal? AlturaReal { get; set; }
    public decimal? LarguraReal { get; set; }
    public decimal? ComprimentoReal { get; set; }
    public bool Ativo { get; private set; }
    public List<ItemPedido> ItensPedido { get; set; } = [];
    public List<Produto> Produtos { get; set; } = new();

    public void InativarAtivar(bool ativo)
    {
        Ativo = ativo;
    }
}