using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Models.TabelaDePrecos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface ITabelaDePrecoService
{
    Task<PaginacaoViewModel<TabelaDePrecoViewModel>> GetPaginacaoTabelaViewModelAsync(PaginacaoTabelaDePrecoDto paginacaoTabelaDePrecoDto);
    Task<TabelaDePrecoViewModel> GetPrecoTabelaViewModelAsync(Guid id);
    Task<TabelaDePrecoViewModel> UpdateTabelaDePrecoAsync(UpdateTabelaDePrecoDto updateTabelaDePrecoDto);
    Task<TabelaDePrecoViewModel> CreateTabelaDePrecoAsync(CreateTabelaDePrecoDto createTabelaDePrecoDto);
    Task DeleteTabelaDePrecoAsync(Guid id);
}
