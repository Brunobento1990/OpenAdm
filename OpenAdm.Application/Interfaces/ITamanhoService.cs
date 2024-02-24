using OpenAdm.Application.Dtos.Tamanhos;
using OpenAdm.Application.Models.Tamanhos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface ITamanhoService
{
    Task<TamanhoViewModel> GetTamanhoViewModelAsync(Guid id);
    Task DeleteTamanhoAsync(Guid id);
    Task<TamanhoViewModel> CreateTamanhoAsync(CreateTamanhoDto createTamanhoDto);
    Task<TamanhoViewModel> UpdateTamanhoAsync(UpdateTamanhoDto updateTamanhoDto);
    Task<PaginacaoViewModel<TamanhoViewModel>> GetPaginacaoAsync(PaginacaoTamanhoDto paginacaoTamanhoDto);
}
