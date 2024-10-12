using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IFaturaContasAReceberService
{
    Task<FaturaContasAReceberViewModel> PagarAsync(PagarFaturaAReceberDto pagarFaturaAReceberDto);
    Task<PaginacaoViewModel<FaturaContasAReceberViewModel>> PaginacaoAsync(PaginacaoFaturaAReceberDto paginacaoFaturaAReceberDto);
    Task<IList<FaturaContasAReceberViewModel>> GetByPedidoIdAsync(Guid pedidoId, StatusFaturaContasAReceberEnum? statusFaturaContasAReceberEnum);
}
