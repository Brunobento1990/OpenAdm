using OpenAdm.Application.Dtos.LojaParceira;
using OpenAdm.Application.Models.LojasParceira;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface ILojasParceirasService
{
    Task<PaginacaoViewModel<LojasParceirasViewModel>> GetPaginacaoAsync(PaginacaoLojasParceirasDto paginacaoLojasParceirasDto);
    Task<LojasParceirasViewModel> GetLojasParceirasViewModelAsync(Guid id);
    Task<IList<string?>> ListLojasParceirasViewModelAsync();
    Task<LojasParceirasViewModel> CreateLojaParceiraAsync(CreateLojaParceiraDto createLojaParceiraDto);
    Task<LojasParceirasViewModel> UpdateLojaParceiraAsync(UpdateLojaParceiraDto updateLojaParceiraDto);
    Task DeleteLojaParceiraAsync(Guid id);
}
