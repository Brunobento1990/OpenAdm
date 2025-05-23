using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Model;

public class ParceiroAutenticado : IParceiroAutenticado
{
    public Guid Id { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
}
