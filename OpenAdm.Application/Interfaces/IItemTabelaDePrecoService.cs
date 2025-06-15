using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Models.TabelaDePrecos;

namespace OpenAdm.Application.Interfaces;

public interface IItemTabelaDePrecoService
{
    Task CreateItemTabelaDePrecoAsync(CreateItensTabelaDePrecoDto createItensTabelaDePrecoDto);
    Task CreateListItemTabelaDePrecoAsync(IList<CreateItensTabelaDePrecoDto> createItensTabelaDePrecoDto);
    Task DeleteItemAsync(Guid id);
    Task<IList<ItensTabelaDePrecoViewModel>> ObterItensDaTabelaDePrecoAsync(Guid tebaleDePrecoId);
}
