using OpenAdm.Application.Models.Pagamentos;

namespace OpenAdm.Application.Interfaces;

public interface IPagamentoService
{
    Task<PagamentoViewModel> GerarPagamentoAsync(Guid pedidoId);
}
