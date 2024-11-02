using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Models.FaturasModel;
using OpenAdm.Application.Models.ParcelasModel;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IParcelaService
{
    Task BaixarFaturaWebHookAsync(NotificationFaturaWebHook notificationFaturaWebHook);
    Task<decimal> GetSumAReceberAsync();
    Task<IList<ParcelaPagaDashBoardModel>> FaturasDashBoardAsync();
    Task<ParcelaViewModel> PagarAsync(PagarParcelaDto pagarFaturaAReceberDto);
    Task<ParcelaViewModel> EditarAsync(ParcelaEditDto parcelaEditDto);
    Task<ParcelaViewModel> AddAsync(ParcelaCriarDto parcelaCriarDto);
    Task<ParcelaViewModel> GetByIdAsync(Guid id);
    Task<ParcelaViewModel> EditAsync(FaturaEdit faturaAReceberEdit);
    Task<bool> ExcluirAsync(Guid id);
    Task<PaginacaoViewModel<ParcelaViewModel>> PaginacaoAsync(PaginacaoParcelaDto paginacaoFaturaAReceberDto);
    Task<IList<ParcelaViewModel>> GetByPedidoIdAsync(Guid pedidoId, StatusParcelaEnum? statusFaturaContasAReceberEnum);
}
