using OpenAdm.Application.Dtos.Tamanhos;
using OpenAdm.Application.Models.Tamanhos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface ITamanhoService
{
    Task<IList<DropDownItemModel>> BuscarDropDownAsync(DropDownFiltro filtro);
    Task<TamanhoViewModel> GetTamanhoViewModelAsync(Guid id);
    Task InativarAtivarAsync(Guid id, bool ativo);
    Task<TamanhoViewModel> CreateTamanhoAsync(CreateTamanhoDto createTamanhoDto);
    Task<TamanhoViewModel> UpdateTamanhoAsync(UpdateTamanhoDto updateTamanhoDto);
    Task<PaginacaoViewModel<TamanhoViewModel>> GetPaginacaoAsync(FilterModel<Tamanho> paginacaoTamanhoDto);
    Task<IList<TamanhoViewModel>> GetTamanhoViewModelsAsync();
    Task<IDictionary<Guid, TamanhoViewModel>> GetTamanhoPorIdsViewModelsAsync(IList<Guid> ids);
}
