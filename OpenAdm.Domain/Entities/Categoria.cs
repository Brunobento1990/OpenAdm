using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Categoria : BaseEntity
{
    public Categoria(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string descricao,
        string? foto,
        string? nomeFoto,
        bool inativoEcommerce)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
        Foto = foto;
        NomeFoto = nomeFoto;
        InativoEcommerce = inativoEcommerce;
    }

    public void InativarAtivarEcommerce()
    {
        InativoEcommerce = !InativoEcommerce;
    }

    public void Update(string descricao, string? foto, string? nomeFoto)
    {
        Descricao = descricao;
        NomeFoto = nomeFoto;
        Foto = foto;
    }

    public string Descricao { get; private set; }
    public string? Foto { get; private set; }
    public string? NomeFoto { get; private set; }
    public bool InativoEcommerce { get; private set; } = false;
    public List<Produto> Produtos { get; set; } = [];
}
