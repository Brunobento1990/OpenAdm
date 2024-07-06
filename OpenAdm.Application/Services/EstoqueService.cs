using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
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
    private readonly ITamanhoRepository _tamanhoRepository;
    private readonly IPesoRepository _pesoRepository;

    public EstoqueService(
        IEstoqueRepository estoqueRepository,
        IMovimentacaoDeProdutoRepository movimentacaoDeProdutoRepository,
        IProdutoRepository produtoRepository,
        ITamanhoRepository tamanhoRepository,
        IPesoRepository pesoRepository)
    {
        _estoqueRepository = estoqueRepository;
        _movimentacaoDeProdutoRepository = movimentacaoDeProdutoRepository;
        _produtoRepository = produtoRepository;
        _tamanhoRepository = tamanhoRepository;
        _pesoRepository = pesoRepository;
    }

    public async Task<EstoqueViewModel> GetEstoqueViewModelAsync(Guid id)
    {
        var estoque = await _estoqueRepository.GetEstoqueAsync(x => x.Id == id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var produto = await _produtoRepository.GetProdutoByIdAsync(estoque.ProdutoId);
        var peso = string.Empty;
        var tamanho = string.Empty;

        if (estoque.TamanhoId != null)
        {
            var tamanhoDb = await _tamanhoRepository.GetTamanhoByIdAsync(estoque.TamanhoId.Value);
            tamanho = tamanhoDb?.Descricao;
        }

        if (estoque.PesoId != null)
        {
            var pesoDb = await _pesoRepository.GetPesoByIdAsync(estoque.PesoId.Value);
            peso = pesoDb?.Descricao;
        }

        return new EstoqueViewModel().ToModel(estoque, produto?.Descricao, tamanho, peso);
    }

    public async Task<PaginacaoViewModel<EstoqueViewModel>> GetPaginacaoAsync(PaginacaoEstoqueDto paginacaoEstoqueDto)
    {
        var paginacao = await _estoqueRepository.GetPaginacaoEstoqueAsync(paginacaoEstoqueDto);
        var produtosids = paginacao
            .Values
            .DistinctBy(x => x.ProdutoId)
            .Select(x => x.ProdutoId)
            .ToList();

        var tamanhosIds = paginacao
            .Values.Where(x => x.TamanhoId != null)
            .DistinctBy(x => x.TamanhoId)
            .Select(x => x.TamanhoId!.Value)
            .ToList();

        var pesosIds = paginacao
            .Values
            .Where(x => x.PesoId != null)
            .DistinctBy(x => x.PesoId)
            .Select(x => x.PesoId!.Value)
            .ToList();

        var produtos = await _produtoRepository.GetDescricaoDeProdutosAsync(produtosids);
        var tamanhos = await _tamanhoRepository.GetDescricaoTamanhosAsync(tamanhosIds);
        var pesos = await _pesoRepository.GetDescricaoPesosAsync(pesosIds);

        return new PaginacaoViewModel<EstoqueViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao
                .Values
                .Select(x =>
                    new EstoqueViewModel()
                        .ToModel(x,
                            produtos.FirstOrDefault(p => p.Key == x.ProdutoId).Value,
                            tamanhos.FirstOrDefault(t => t.Key == x.TamanhoId).Value,
                            pesos.FirstOrDefault(p => p.Key == x.PesoId).Value))
                .ToList()
        };
    }

    public async Task<bool> MovimentacaoDeProdutoAsync(MovimentacaoDeProdutoDto movimentacaoDeProdutoDto)
    {
        var estoque = await GetEstoqueLocalAsync(
            movimentacaoDeProdutoDto.ProdutoId,
            movimentacaoDeProdutoDto.PesoId,
            movimentacaoDeProdutoDto.TamanhoId);

        var date = DateTime.Now;

        if (estoque == null)
        {
            var newQuantidade = movimentacaoDeProdutoDto.TipoMovimentacaoDeProduto == TipoMovimentacaoDeProduto.Saida ?
                -movimentacaoDeProdutoDto.Quantidade : movimentacaoDeProdutoDto.Quantidade;

            estoque = new Estoque(
                Guid.NewGuid(),
                date,
                date,
                0,
                movimentacaoDeProdutoDto.ProdutoId,
                newQuantidade,
                movimentacaoDeProdutoDto.TamanhoId,
                movimentacaoDeProdutoDto.PesoId);

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
            movimentacaoDeProdutoDto.ProdutoId,
            movimentacaoDeProdutoDto.TamanhoId,
            movimentacaoDeProdutoDto.PesoId,
            movimentacaoDeProdutoDto.Observacao);

        await _movimentacaoDeProdutoRepository.AddAsync(movimento);

        return true;
    }

    public async Task<bool> UpdateEstoqueAsync(UpdateEstoqueDto updateEstoqueDto)
    {
        var estoque = await _estoqueRepository.GetEstoqueAsync(x => x.ProdutoId == updateEstoqueDto.ProdutoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        estoque.UpdateEstoqueAtual(updateEstoqueDto.Quantidade);

        await _estoqueRepository.UpdateAsync(estoque);

        return true;
    }

    private async Task<Estoque?> GetEstoqueLocalAsync(Guid produtoId, Guid? pesoId, Guid? tamanhoId)
    {
        return await _estoqueRepository
                .GetEstoqueAsync(x => x.ProdutoId == produtoId &&
                    x.PesoId == pesoId &&
                    x.TamanhoId == tamanhoId);
    }
}
