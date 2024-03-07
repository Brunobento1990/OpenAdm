using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IMovimentacaoDeProdutosService
{
    Task<PaginacaoViewModel<MovimentacaoDeProdutoViewModel>> 
        GetPaginacaoAsync(PaginacaoMovimentacaoDeProdutoDto paginacaoMovimentacaoDeProdutoDto);
}
