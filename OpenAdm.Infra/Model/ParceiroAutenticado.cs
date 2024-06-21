using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Model;

public class ParceiroAutenticado : IParceiroAutenticado
{
    public Guid XApi { get ; set ; }
    public string StringConnection { get; set; } = string.Empty;
}
