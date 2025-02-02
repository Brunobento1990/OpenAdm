using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IFaturaRepository : IGenericRepository<Fatura>
{
    Task<Fatura?> GetByIdAsync(Guid id);
    Task<Fatura?> GetByIdCompletaAsync(Guid id);
    Task<Fatura?> GetByPedidoIdAsync(Guid id);
    Task EditarAsync(Fatura fatura);
    void ExcluirParcelasAsync(IList<Parcela> parcelas);
}
