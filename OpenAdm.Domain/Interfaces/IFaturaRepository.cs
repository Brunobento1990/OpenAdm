using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IFaturaRepository : IGenericRepository<Fatura>
{
    Task<Fatura?> GetByIdAsync(Guid id);
    Task<Fatura?> GetByIdCompletaAsync(Guid id);
    Task<Fatura?> ObterParaRenegociarAsync(Guid id);
    Task AdicionarParcelasAsync(IEnumerable<Parcela> parcelas);
    Task AddHistoricoAsync(FaturaHistorico historico);
}
