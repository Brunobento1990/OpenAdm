using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IFaturaService
{
    Task<ResultPartner<ResultadoPadraoViewModel>> NegociarCobrancaAsync(NegociarCobrancaPedidoDto dto);
    Task<ResultPartner<ResultadoPadraoViewModel>> BaixaAutomaticaAsync(BaixaAutomaticaDto dto);
    Task CriarContasAReceberAsync(CriarFaturaDto contasAReceberDto);
    Task VerificarFechamentoAsync(Guid id);
    Task<FaturaViewModel> CriarAdmAsync(FaturaCriarAdmDto faturaCriarAdmDto);
    Task<FaturaViewModel> GetCompletaAsync(Guid id);
    Task<FaturaViewModel> GetByIdAsync(Guid id);
}
