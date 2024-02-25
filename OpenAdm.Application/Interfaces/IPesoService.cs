using OpenAdm.Application.Dtos.Pesos;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IPesoService
{
    Task<PesoViewModel> GetPesoViewModelAsync(Guid id);
    Task DeletePesoAsync(Guid id);
    Task<PesoViewModel> CreatePesoAsync(CreatePesoDto createPesoDto);
    Task<PesoViewModel> UpdatePesoAsync(UpdatePesoDto updatePesoDto);
    Task<PaginacaoViewModel<PesoViewModel>> GetPaginacaoAsync(PaginacaoPesoDto paginacaoPesoDto);
    Task<IList<PesoViewModel>> GetPesosViewModelAsync();
}
