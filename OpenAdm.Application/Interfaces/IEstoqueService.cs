using OpenAdm.Application.Dtos.Estoques;
using OpenAdm.Application.Models.Estoques;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IEstoqueService
{
    Task<bool> MovimentacaoDeProdutoAsync(MovimentacaoDeProdutoDto movimentacaoDeProdutoDto);
    Task<PaginacaoViewModel<EstoqueViewModel>> GetPaginacaoAsync(PaginacaoEstoqueDto paginacaoEstoqueDto);
}
