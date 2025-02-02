using OpenAdm.Application.Dtos.Pesos;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IPesoService
{
    Task<PesoViewModel> GetPesoViewModelAsync(Guid id);
    Task DeletePesoAsync(Guid id);
    Task<PesoViewModel> CreatePesoAsync(CreatePesoDto createPesoDto);
    Task<PesoViewModel> UpdatePesoAsync(UpdatePesoDto updatePesoDto);
    Task<PaginacaoViewModel<PesoViewModel>> GetPaginacaoAsync(FilterModel<Peso> paginacaoPesoDto);
    Task<IList<PesoViewModel>> GetPesosViewModelAsync();
    Task<IDictionary<Guid, PesoViewModel>> GetPesosByPesosIdsViewModelAsync(IList<Guid> pesosIds);
}
