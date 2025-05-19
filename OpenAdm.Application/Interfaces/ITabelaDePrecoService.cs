using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Models.TabelaDePrecos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ITabelaDePrecoService
{
    Task<PaginacaoViewModel<TabelaDePrecoViewModel>> GetPaginacaoTabelaViewModelAsync(FilterModel<TabelaDePreco> paginacaoTabelaDePrecoDto);
    Task<TabelaDePrecoViewModel> GetPrecoTabelaViewModelAsync(Guid id);
    Task<TabelaDePrecoViewModel> GetTabelaViewModelByProdutoIdAsync(Guid? produtoId = null);
    Task<TabelaDePrecoViewModel> UpdateTabelaDePrecoAsync(UpdateTabelaDePrecoDto updateTabelaDePrecoDto);
    Task<TabelaDePrecoViewModel> CreateTabelaDePrecoAsync(CreateTabelaDePrecoDto createTabelaDePrecoDto);
    Task DeleteTabelaDePrecoAsync(Guid id);
    Task<IList<TabelaDePrecoViewModel>> GetAllTabelaDePrecoViewModelAsync();
    Task<TabelaDePrecoViewModel> GetTabelaDePrecoViewModelAtivaAsync();
}
