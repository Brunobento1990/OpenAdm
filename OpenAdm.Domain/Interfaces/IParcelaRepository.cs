using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Interfaces;

public interface IParcelaRepository : IGenericRepository<Parcela>
{
    Task<decimal> SumTotalAsync(TipoFaturaEnum faturaEnum);
    Task<IDictionary<int, decimal>> SumTotalMesesAsync(TipoFaturaEnum faturaEnum);
    Task<Parcela?> GetByIdAsync(Guid id);
    Task<Parcela?> GetByIdExternoAsync(string idExterno);
    Task<IList<Parcela>> GetByPedidoIdAsync(Guid pedidoId, StatusParcelaEnum? statusFaturaContasAReceberEnum);
}
