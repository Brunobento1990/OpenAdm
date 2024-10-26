using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IFaturaContasAReceberService
{
    Task BaixarFaturaWebHookAsync(NotificationFaturaWebHook notificationFaturaWebHook);
    Task<decimal> GetSumAReceberAsync();
    Task<IList<FaturaPagaDashBoardModel>> FaturasDashBoardAsync();
    Task<FaturaContasAReceberViewModel> PagarAsync(PagarFaturaAReceberDto pagarFaturaAReceberDto);
    Task<FaturaContasAReceberViewModel> GetByIdAsync(Guid id);
    Task<FaturaContasAReceberViewModel> EditAsync(FaturaAReceberEdit faturaAReceberEdit);
    Task<PaginacaoViewModel<FaturaContasAReceberViewModel>> PaginacaoAsync(PaginacaoFaturaAReceberDto paginacaoFaturaAReceberDto);
    Task<IList<FaturaContasAReceberViewModel>> GetByPedidoIdAsync(Guid pedidoId, StatusFaturaContasAReceberEnum? statusFaturaContasAReceberEnum);
}
