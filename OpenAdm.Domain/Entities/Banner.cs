using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class Banner : BaseEntityParceiro
{
    public Banner(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string foto,
        bool ativo,
        string nomeFoto,
        Guid parceiroId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero, parceiroId)
    {
        Foto = foto;
        Ativo = ativo;
        NomeFoto = nomeFoto;
    }

    public string Foto { get; private set; }
    public string NomeFoto { get; private set; }
    public bool Ativo { get; private set; }

    public void Update(string foto, string nomeFoto, bool? ativo)
    {
        NomeFoto = nomeFoto;
        Foto = foto;
        Ativo = ativo != null ? ativo.Value : Ativo;
    }
}
