using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Produto : BaseEntity
{
    public Produto(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string descricao,
        string? especificacaoTecnica,
        Guid categoriaId,
        string? referencia,
        string? urlFoto,
        string? nomeFoto)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
        EspecificacaoTecnica = especificacaoTecnica;
        CategoriaId = categoriaId;
        Referencia = referencia;
        UrlFoto = urlFoto;
        NomeFoto = nomeFoto;
    }

    public string Descricao { get; private set; }
    public string? EspecificacaoTecnica { get; private set; }
    public List<Tamanho> Tamanhos { get; set; } = new();
    public List<Peso> Pesos { get; set; } = new();
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; set; } = null!;
    public List<ItemPedido> ItensPedido { get; set; } = new();
    public List<ItemTabelaDePreco> ItensTabelaDePreco { get; set; } = new();
    public string? Referencia { get; private set; }
    public string? UrlFoto { get; private set; }
    public string? NomeFoto { get; private set; }
    public void Update(
        string descricao,
        string? especificacaoTecnica,
        Guid categoriaId,
        string? referencia,
        string? urlFoto,
        string? nomeFoto)
    {
        UrlFoto = urlFoto;
        Descricao = descricao;
        EspecificacaoTecnica = especificacaoTecnica;
        CategoriaId = categoriaId;
        Referencia = referencia;
        NomeFoto = nomeFoto;
    }
}
