using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Domain.Model;
using OpenAdm.Application.Dtos.MovimentosDeProdutos;

namespace OpenAdm.Application.Interfaces;

public interface IMovimentacaoDeProdutosService
{
    Task<PaginacaoViewModel<MovimentacaoDeProdutoViewModel>>
        GetPaginacaoAsync(FilterModel<MovimentacaoDeProduto> paginacaoMovimentacaoDeProdutoDto);
    Task MovimentarItensPedidoAsync(IList<ItemPedido> itens);
    Task<IList<MovimentoDeProdutoDashBoardModel>> MovimentoDashBoardAsync();
    Task<byte[]> GerarRelatorioAsync(RelatorioMovimentoDeProdutoDto relatorioMovimentoDeProdutoDto);
}
