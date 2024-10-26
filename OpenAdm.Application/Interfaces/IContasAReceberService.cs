using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Interfaces;

public interface IContasAReceberService
{
    Task CriarContasAReceberAsync(CriarContasAReceberDto contasAReceberDto);
    Task VerificarFechamentoAsync(Guid id);
    Task<PagamentoViewModel> GerarPagamentoAsync(MeioDePagamentoEnum meioDePagamento, Guid pedidoId);
}
