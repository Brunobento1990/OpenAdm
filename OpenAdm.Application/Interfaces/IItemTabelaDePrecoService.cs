using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Models.TabelaDePrecos;
using OpenAdm.Application.Models.Representantes;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface IItemTabelaDePrecoService
{
    Task<PaginacaoViewModel<ItemCatalogoRepresentanteViewModel>> PaginarCatalogoRepresentanteAsync(
        FilterModel<ItemTabelaDePreco> filtro);
    Task<ItensTabelaDePrecoViewModel> CreateItemTabelaDePrecoAsync(CreateItensTabelaDePrecoDto createItensTabelaDePrecoDto);
    Task CreateListItemTabelaDePrecoAsync(IList<CreateItensTabelaDePrecoDto> createItensTabelaDePrecoDto);
    Task DeleteItemAsync(Guid id);
    Task<IList<ItensTabelaDePrecoViewModel>> ObterItensDaTabelaDePrecoAsync(Guid tebaleDePrecoId);
    Task UpdatePrecoPorPesoAsync(UpdateItensTabelaDePrecoPorPesoDto updateItensTabelaDePrecoPorPesoDto);
    Task UpdatePrecoPorTamanhoAsync(UpdateItensTabelaDePrecoPorTamanhoDto updateItensTabelaDePrecoPorTamanhoDto);
    Task<ItensTabelaDePrecoViewModel> UpdateValoresAsync(UpdateItemTabelaDePrecoDto updateItemTabelaDePrecoDto);
}
