using OpenAdm.Application.Dtos.TabelasDePrecos;

namespace OpenAdm.Application.Interfaces;

public interface IItemTabelaDePrecoService
{
    Task CreateItemTabelaDePrecoAsync(CreateItensTabelaDePrecoDto createItensTabelaDePrecoDto);
    Task CreateListItemTabelaDePrecoAsync(IList<CreateItensTabelaDePrecoDto> createItensTabelaDePrecoDto);
    Task DeleteItemAsync(Guid id);
}
