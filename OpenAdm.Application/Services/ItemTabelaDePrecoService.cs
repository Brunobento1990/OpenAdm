using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ItemTabelaDePrecoService : IItemTabelaDePrecoService
{
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;

    public ItemTabelaDePrecoService(IItemTabelaDePrecoRepository itemTabelaDePrecoRepository)
    {
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
    }

    public async Task CreateItemTabelaDePrecoAsync(CreateItensTabelaDePrecoDto createItensTabelaDePrecoDto)
    {
        var itemTabelaDePreco = createItensTabelaDePrecoDto.ToEntity();

        await _itemTabelaDePrecoRepository.AddAsync(itemTabelaDePreco);
    }

    public async Task CreateListItemTabelaDePrecoAsync(IList<CreateItensTabelaDePrecoDto> createItensTabelaDePrecoDto)
    {
        if (createItensTabelaDePrecoDto.Count == 0) return;

        var itensTabelaDePreco = createItensTabelaDePrecoDto.Select(x => x.ToEntity()).ToList();

        await _itemTabelaDePrecoRepository.DeleteItensTabelaDePrecoByProdutoIdAsync(createItensTabelaDePrecoDto.First().ProdutoId);

        await _itemTabelaDePrecoRepository.AddRangeAsync(itensTabelaDePreco);
    }

    public async Task DeleteItemAsync(Guid id)
    {
        var item = await _itemTabelaDePrecoRepository.GetItemTabelaDePrecoByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o item da tabela de preço!");


        await _itemTabelaDePrecoRepository.DeleteAsync(item);
    }
}
