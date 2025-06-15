using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities.OpenAdm;

public sealed class TelefoneParceiro : BaseTelefone
{
    public TelefoneParceiro(Guid id, string telefone, Guid parceiroId)
        : base(id, telefone)
    {
        ParceiroId = parceiroId;
    }

    public Guid ParceiroId { get; private set; }
    public Parceiro Parceiro { get; set; } = null!;

    public static TelefoneParceiro NovoTelefone(string telefone, Guid parceiroId)
    {
        return new TelefoneParceiro(Guid.NewGuid(), telefone, parceiroId);
    }
}
