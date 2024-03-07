using Domain.Pkg.Entities;
using OpenAdm.Application.Dtos.Estoques;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Estoques;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class EstoqueService : IEstoqueService
{
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IMovimentacaoDeProdutoRepository _movimentacaoDeProdutoRepository;
    private readonly IProdutoRepository _produtoRepository;

    public EstoqueService(IEstoqueRepository estoqueRepository, IMovimentacaoDeProdutoRepository movimentacaoDeProdutoRepository, IProdutoRepository produtoRepository)
    {
        _estoqueRepository = estoqueRepository;
        _movimentacaoDeProdutoRepository = movimentacaoDeProdutoRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<PaginacaoViewModel<EstoqueViewModel>> GetPaginacaoAsync(PaginacaoEstoqueDto paginacaoEstoqueDto)
    {
        var paginacao = await _estoqueRepository.GetPaginacaoEstoqueAsync(paginacaoEstoqueDto);
        var produtosids = paginacao.Values.DistinctBy(x => x.ProdutoId).Select(x => x.ProdutoId).ToList();
        var produtos = await _produtoRepository.GetProdutosByListIdAsync(produtosids);

        return new PaginacaoViewModel<EstoqueViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao
                .Values
                .Select(x =>
                    new EstoqueViewModel()
                        .ToModel(x, produtos.FirstOrDefault(p => p.Id == x.ProdutoId)?.Descricao))
                .ToList()
        };
    }

    public async Task<bool> MovimentacaoDeProdutoAsync(MovimentacaoDeProdutoDto movimentacaoDeProdutoDto)
    {
        var estoque = await _estoqueRepository
            .GetEstoqueByProdutoIdAsync(movimentacaoDeProdutoDto.ProdutoId);

        var date = DateTime.Now;

        if (estoque == null)
        {
            estoque = new Estoque(
                Guid.NewGuid(),
                date,
                date,
                0,
                movimentacaoDeProdutoDto.ProdutoId,
                movimentacaoDeProdutoDto.Quantidade);

            await _estoqueRepository.AddAsync(estoque);
        }
        else
        {
            estoque.UpdateEstoque(movimentacaoDeProdutoDto.Quantidade, movimentacaoDeProdutoDto.TipoMovimentacaoDeProduto);
            await _estoqueRepository.UpdateAsync(estoque);
        }

        var movimento = new MovimentacaoDeProduto(
            Guid.NewGuid(),
            date,
            date,
            0,
            movimentacaoDeProdutoDto.Quantidade,
            movimentacaoDeProdutoDto.TipoMovimentacaoDeProduto,
            movimentacaoDeProdutoDto.ProdutoId);

        await _movimentacaoDeProdutoRepository.AddAsync(movimento);

        return true;
    }
}
