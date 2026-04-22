using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IParcelaCobrancaRepository
{
    Task<ParcelaCobranca?> ObterPorIdAsync(Guid id);
    Task<ParcelaCobranca?> ObterPorIdAsNoTrackingAsync(Guid id);
    Task<ParcelaCobranca?> ObterPorIdExternoAsync(string idExterno);
    Task AddAsync(ParcelaCobranca parcelaCobranca);
    void Update(ParcelaCobranca parcelaCobranca);
    Task SaveChangesAsync();
    Task<int> ProximoNumeroParcela(Guid empresaOpenAdmId);
    Task<bool> TemCobrancaAsync(Guid empresaOpenAdmId, int mes, int ano);
    Task<PaginacaoViewModel<ParcelaCobranca>> PaginacaoAsync(FilterModel<ParcelaCobranca> filterModel);
    Task UpdateIdExternoAsync(Guid id, string idExterno);
}