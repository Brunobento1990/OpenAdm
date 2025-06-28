using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.TabelaDePrecos;
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

    public async Task<IList<ItensTabelaDePrecoViewModel>> ObterItensDaTabelaDePrecoAsync(Guid tebaleDePrecoId)
    {
        var itens = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdAsync(tebaleDePrecoId);
        return itens.Select(x => new ItensTabelaDePrecoViewModel()
        {
            Id = x.Id,
            Numero = x.Numero,
            PesoId = x.PesoId,
            TamanhoId = x.TamanhoId,
            ProdutoId = x.ProdutoId,
            ValorUnitarioAtacado = x.ValorUnitarioAtacado,
            ValorUnitarioVarejo = x.ValorUnitarioVarejo
        }).ToList();
    }

    public async Task UpdatePrecoPorPesoAsync(UpdateItensTabelaDePrecoPorPesoDto updateItensTabelaDePrecoPorPesoDto)
    {
        var itens = await _itemTabelaDePrecoRepository.ObterPorPesoIdAsync(updateItensTabelaDePrecoPorPesoDto.PesoId);
        if (itens.Count == 0)
        {
            throw new ExceptionApi("Não há itens a serem atualizados");
        }

        foreach (var item in itens)
        {
            item.Update(
                valorUnitarioAtacado: updateItensTabelaDePrecoPorPesoDto.ValorUnitarioAtacado,
                valorUnitarioVarejo: updateItensTabelaDePrecoPorPesoDto.ValorUnitarioVarejo,
                tamanhoId: item.TamanhoId,
                pesoId: item.PesoId,
                produtoId: item.ProdutoId);

            _itemTabelaDePrecoRepository.Update(item);
        }

        await _itemTabelaDePrecoRepository.SaveChangesAsync();
    }

    public async Task UpdatePrecoPorTamanhoAsync(UpdateItensTabelaDePrecoPorTamanhoDto updateItensTabelaDePrecoPorTamanhoDto)
    {
        var itens = await _itemTabelaDePrecoRepository.ObterPorTamanhoIdAsync(updateItensTabelaDePrecoPorTamanhoDto.TamanhoId);
        if (itens.Count == 0)
        {
            throw new ExceptionApi("Não há itens a serem atualizados");
        }

        foreach (var item in itens)
        {
            item.Update(
                valorUnitarioAtacado: updateItensTabelaDePrecoPorTamanhoDto.ValorUnitarioAtacado,
                valorUnitarioVarejo: updateItensTabelaDePrecoPorTamanhoDto.ValorUnitarioVarejo,
                tamanhoId: item.TamanhoId,
                pesoId: item.PesoId,
                produtoId: item.ProdutoId);

            _itemTabelaDePrecoRepository.Update(item);
        }

        await _itemTabelaDePrecoRepository.SaveChangesAsync();
    }
}
