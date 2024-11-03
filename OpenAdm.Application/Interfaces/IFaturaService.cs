using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Interfaces;

public interface IFaturaService
{
    Task CriarContasAReceberAsync(CriarFaturaDto contasAReceberDto);
    Task VerificarFechamentoAsync(Guid id);
    Task<PagamentoViewModel> GerarPagamentoAsync(MeioDePagamentoEnum meioDePagamento, Guid pedidoId);
    Task<FaturaViewModel> CriarAdmAsync(FaturaCriarAdmDto faturaCriarAdmDto);
    Task<FaturaViewModel> GetCompletaAsync(Guid id);
    Task<FaturaViewModel> GetByIdAsync(Guid id);
}
