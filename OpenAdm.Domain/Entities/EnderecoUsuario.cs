using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class EnderecoUsuario : BaseEndereco
{
    public EnderecoUsuario(
        string cep,
        string logradouro,
        string bairro,
        string localidade,
        string complemento,
        string numero,
        string uf,
        Guid id,
        Guid usuarioId)
            : base(cep, logradouro, bairro, localidade, complemento, numero, uf)
    {
        Id = id;
        UsuarioId = usuarioId;
    }

    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; set; } = null!;
}
