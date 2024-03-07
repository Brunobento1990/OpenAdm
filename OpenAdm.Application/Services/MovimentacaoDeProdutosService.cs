using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class MovimentacaoDeProdutosService : IMovimentacaoDeProdutosService
{
    private readonly IMovimentacaoDeProdutoRepository _movimentacaoDeProdutorepository;
    private readonly IProdutoRepository _produtoRepository;

    public MovimentacaoDeProdutosService(
        IMovimentacaoDeProdutoRepository movimentacaoDeProdutorepository, 
        IProdutoRepository produtoRepository)
    {
        _movimentacaoDeProdutorepository = movimentacaoDeProdutorepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<PaginacaoViewModel<MovimentacaoDeProdutoViewModel>> 
        GetPaginacaoAsync(PaginacaoMovimentacaoDeProdutoDto paginacaoMovimentacaoDeProdutoDto)
    {
        var paginacao = await _movimentacaoDeProdutorepository.GetPaginacaoMovimentacaoDeProdutoAsync(paginacaoMovimentacaoDeProdutoDto);
        var produtosIds = paginacao
            .Values
            .DistinctBy(x => x.ProdutoId)
            .Select(x => x.ProdutoId)
            .ToList();

        var produtos = await _produtoRepository.GetProdutosByListIdAsync(produtosIds);

        return new PaginacaoViewModel<MovimentacaoDeProdutoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao
                .Values
                .Select(x => 
                    new MovimentacaoDeProdutoViewModel().ToModel(x, produtos.FirstOrDefault(p => p.Id == x.ProdutoId)?.Descricao))
                .ToList()
        };
    }
}
