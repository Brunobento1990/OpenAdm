using OpenAdm.Domain.Extensions;

namespace OpenAdm.Domain.Entities.Bases;

public abstract class BaseTelefone
{
    protected BaseTelefone(Guid id, string telefone)
    {
        Id = id;
        Telefone = telefone;
    }

    public Guid Id { get; protected set; }
    public string Telefone { get; protected set; }

    public void Editar(string telefone)
    {
        Telefone = telefone.LimparMascaraTelefone();
    }
}
