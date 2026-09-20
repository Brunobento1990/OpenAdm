using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Test.Domain.Builder;

public class ParcelaBuilder
{
    private Guid _faturaId = Guid.NewGuid();
    private string? _idExterno;

    public static ParcelaBuilder Init() => new();

    public ParcelaBuilder ComFaturaId(Guid faturaId)
    {
        _faturaId = faturaId;
        return this;
    }

    public ParcelaBuilder ComIdExterno(string idExterno)
    {
        _idExterno = idExterno;
        return this;
    }

    public Parcela Build() => Parcela.NovaFatura(
        DateTime.UtcNow.AddDays(30),
        1,
        null,
        100,
        null,
        _faturaId,
        _idExterno,
        null,
        null,
        TipoFaturaEnum.AReceber);
}
