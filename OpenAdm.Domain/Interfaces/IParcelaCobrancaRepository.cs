using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Domain.Interfaces;

public interface IParcelaCobrancaRepository
{
    Task AddAsync(ParcelaCobranca parcelaCobranca);
    void Update(ParcelaCobranca parcelaCobranca);
    Task SaveChangesAsync();
    Task<int> ProximoNumeroParcela(Guid empresaOpenAdmId);
    Task<bool> TemCobrancaAsync(Guid empresaOpenAdmId, int mes, int ano);
}