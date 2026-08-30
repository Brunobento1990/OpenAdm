using OpenAdm.Application.Dtos.Pesos;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface IPesoService
{
    Task<IList<DropDownItemModel>> BuscarDropDownAsync(DropDownFiltro filtro);
    Task<PesoViewModel> GetPesoViewModelAsync(Guid id);
    Task InativarAtivarAsync(Guid id, bool ativo);
    Task DeletePesoAsync(Guid id);
    Task<PesoViewModel> CreatePesoAsync(CreatePesoDto createPesoDto);
    Task<PesoViewModel> UpdatePesoAsync(UpdatePesoDto updatePesoDto);
    Task<PaginacaoViewModel<PesoViewModel>> GetPaginacaoAsync(FilterModel<Peso> paginacaoPesoDto);
    Task<IList<PesoViewModel>> GetPesosViewModelAsync();
    Task<IDictionary<Guid, PesoViewModel>> GetPesosByPesosIdsViewModelAsync(IList<Guid> pesosIds);
}
