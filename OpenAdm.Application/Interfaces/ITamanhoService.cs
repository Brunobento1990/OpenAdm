using OpenAdm.Application.Dtos.Tamanhos;
using OpenAdm.Application.Models.Tamanhos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ITamanhoService
{
    Task<TamanhoViewModel> GetTamanhoViewModelAsync(Guid id);
    Task DeleteTamanhoAsync(Guid id);
    Task<TamanhoViewModel> CreateTamanhoAsync(CreateTamanhoDto createTamanhoDto);
    Task<TamanhoViewModel> UpdateTamanhoAsync(UpdateTamanhoDto updateTamanhoDto);
    Task<PaginacaoViewModel<TamanhoViewModel>> GetPaginacaoAsync(FilterModel<Tamanho> paginacaoTamanhoDto);
    Task<IList<TamanhoViewModel>> GetTamanhoViewModelsAsync();
    Task<IDictionary<Guid, TamanhoViewModel>> GetTamanhoPorIdsViewModelsAsync(IList<Guid> ids);
}
