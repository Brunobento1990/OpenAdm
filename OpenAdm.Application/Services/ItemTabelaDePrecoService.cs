using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;
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

    public async Task DeleteItemAsync(Guid id)
    {
        var item = await _itemTabelaDePrecoRepository.GetItemTabelaDePrecoByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);


        await _itemTabelaDePrecoRepository.DeleteAsync(item);
    }
}
