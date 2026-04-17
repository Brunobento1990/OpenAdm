using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Worker;

public class ParceiroAutenticadoWorker : IParceiroAutenticado
{
    public Guid Id { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
    public Task<Parceiro> ObterParceiroAutenticadoAsync()
    {
        throw new NotImplementedException();
    }
}