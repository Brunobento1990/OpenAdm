using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.FaturasModel;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface IFaturaService
{
    Task<ResultPartner<ResultadoPadraoViewModel>> NegociarCobrancaAsync(NegociarCobrancaPedidoDto dto);
    Task<ResultPartner<ResultadoPadraoViewModel>> BaixaAutomaticaAsync(BaixaAutomaticaDto dto);
    Task CriarContasAReceberAsync(CriarFaturaDto contasAReceberDto);
    Task<ResultPartner<ResultadoPadraoViewModel>> CriarBonificadaAsync(BaixaAutomaticaDto dto);
    Task<PaginacaoViewModel<FaturaBonificadaPaginacaoViewModel>> PaginacaoBonificadasAsync(FilterModel<Fatura> dto);
    Task VerificarFechamentoAsync(Guid id);
    Task<FaturaViewModel> CriarAdmAsync(FaturaCriarAdmDto faturaCriarAdmDto);
    Task<FaturaViewModel> GetCompletaAsync(Guid id);
    Task<FaturaViewModel> GetByIdAsync(Guid id);
}
