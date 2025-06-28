using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IConfiguracaoDePagamentoRepository
{
    Task<ConfiguracaoDePagamento?> GetAsync(Guid parceiroId);
    Task AddAsync(ConfiguracaoDePagamento configuracaoDePagamento);
    Task UpdateAsync(ConfiguracaoDePagamento configuracaoDePagamento);
}
