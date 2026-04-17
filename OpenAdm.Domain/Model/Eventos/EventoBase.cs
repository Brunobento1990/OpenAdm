using System.Text.Json;

namespace OpenAdm.Domain.Model.Eventos;

public abstract class EventoBase
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType());
    }
}