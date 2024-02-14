using System.Text.Json.Serialization;

namespace OpenAdm.Domain.Entities;

public class Categoria : BaseEntity
{
    [JsonConstructor]
    public Categoria(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, string descricao, byte[]? foto) 
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Descricao = descricao;
        Foto = foto;
    }



    public string Descricao { get; private set; }
    public byte[]? Foto { get; private set; }
    public List<Produto> Produtos { get; set; } = new();
}
