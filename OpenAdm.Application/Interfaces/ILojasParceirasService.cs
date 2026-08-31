using OpenAdm.Application.Dtos.LojasParceiras;
using OpenAdm.Application.Models.LojasParceira;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface ILojasParceirasService
{
    Task InativarAsync(Guid id, bool ativo);

    Task<PaginacaoViewModel<LojasParceirasViewModel>> GetPaginacaoAsync(
        FilterModel<LojaParceira> paginacaoLojasParceirasDto);

    Task<LojasParceirasViewModel> GetLojasParceirasViewModelAsync(Guid id);
    Task<IList<string?>> ListLojasParceirasViewModelAsync();
    Task<IEnumerable<LojasParceirasViewModel>> TodasLojasAsync();
    Task<LojasParceirasViewModel> CreateLojaParceiraAsync(CreateLojaParceiraDto createLojaParceiraDto);
    Task<LojasParceirasViewModel> UpdateLojaParceiraAsync(UpdateLojaParceiraDto updateLojaParceiraDto);
    Task DeleteLojaParceiraAsync(Guid id);
}