using OpenAdm.Application.Dtos.LojasParceiras;
using OpenAdm.Application.Models.LojasParceira;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ILojasParceirasService
{
    Task<PaginacaoViewModel<LojasParceirasViewModel>> GetPaginacaoAsync(FilterModel<LojaParceira> paginacaoLojasParceirasDto);
    Task<LojasParceirasViewModel> GetLojasParceirasViewModelAsync(Guid id);
    Task<IList<string?>> ListLojasParceirasViewModelAsync();
    Task<IList<LojasParceirasViewModel>> TodasLojasAsync();
    Task<LojasParceirasViewModel> CreateLojaParceiraAsync(CreateLojaParceiraDto createLojaParceiraDto);
    Task<LojasParceirasViewModel> UpdateLojaParceiraAsync(UpdateLojaParceiraDto updateLojaParceiraDto);
    Task DeleteLojaParceiraAsync(Guid id);
}
