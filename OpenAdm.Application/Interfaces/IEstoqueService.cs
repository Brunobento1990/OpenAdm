using OpenAdm.Application.Dtos.Estoques;
using OpenAdm.Application.Models.Estoques;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IEstoqueService
{
    Task<bool> MovimentacaoDeProdutoAsync(MovimentacaoDeProdutoDto movimentacaoDeProdutoDto);
    Task<bool> MovimentacaoDePedidoEntregueAsync(IList<ItemPedido> itens);
    Task<PaginacaoViewModel<EstoqueViewModel>> GetPaginacaoAsync(FilterModel<Estoque> paginacaoEstoqueDto);
    Task<bool> UpdateEstoqueAsync(UpdateEstoqueDto updateEstoqueDto);
    Task<EstoqueViewModel> GetEstoqueViewModelAsync(Guid id);
    Task<IList<EstoqueViewModel>> GetPosicaoDeEstoqueAsync();
}
