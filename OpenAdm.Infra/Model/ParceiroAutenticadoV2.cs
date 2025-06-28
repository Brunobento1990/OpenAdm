using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Model;

public class ParceiroAutenticadoV2 : IParceiroAutenticado
{
    public Guid Id { get; set; }
    public string ConnectionString { get; set; } = string.Empty;

    public Task<Parceiro> ObterParceiroAutenticadoAsync()
    {
        throw new NotImplementedException();
    }
}
