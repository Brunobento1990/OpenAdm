using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Interfaces;

public interface IParcelaRepository : IGenericRepository<Parcela>
{
    Task<IList<Parcela>> ListaParcelasTotalizadorAsync(TipoFaturaEnum tipoFatura);
    Task<IDictionary<int, decimal>> SumTotalMesesAsync(TipoFaturaEnum faturaEnum);
    Task<Parcela?> GetByIdAsync(Guid id);
    Task<Parcela?> GetByIdExternoAsync(string idExterno);
    Task<IList<Parcela>> GetByPedidoIdAsync(Guid pedidoId);
    Task AdicionarTransacaoAsync(TransacaoFinanceira transacaoFinanceira);
}
