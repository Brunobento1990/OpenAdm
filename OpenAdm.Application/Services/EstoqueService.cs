using OpenAdm.Application.Dtos.Estoques;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Estoques;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

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
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro de estoque!");

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

        return new EstoqueViewModel().ToModel(estoque, produto?.Descricao, tamanho, peso, produto?.UrlFoto);
    }

    public async Task<PaginacaoViewModel<EstoqueViewModel>> GetPaginacaoAsync(FilterModel<Estoque> paginacaoEstoqueDto)
    {
        var paginacao = await _estoqueRepository.PaginacaoAsync(paginacaoEstoqueDto);

        return new PaginacaoViewModel<EstoqueViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => (EstoqueViewModel)x).ToList()
        };
    }

    public async Task<IList<EstoqueViewModel>> GetPosicaoDeEstoqueAsync()
    {
        var estoquesViewModel = new List<EstoqueViewModel>();

        var estoques = await _estoqueRepository.GetPosicaoEstoqueAsync();

        foreach (var estoque in estoques)
        {
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

            estoquesViewModel.Add(new EstoqueViewModel().ToModel(estoque, produto?.Descricao, tamanho, peso, produto?.UrlFoto));
        }

        return estoquesViewModel;
    }

    public async Task<bool> MovimentacaoDePedidoEntregueAsync(IList<ItemPedido> itens)
    {
        var estoques = new List<Estoque>();

        foreach (var item in itens)
        {
            var estoque = estoques
                .FirstOrDefault(x => x.ProdutoId == item.ProdutoId &&
                    x.PesoId == item.PesoId &&
                    x.TamanhoId == item.TamanhoId);

            if (estoque == null)
            {
                estoque = await GetEstoqueLocalAsync(
                    item.ProdutoId,
                    item.PesoId,
                    item.TamanhoId);

                if (estoque == null)
                {
                    estoque ??= new Estoque(
                        Guid.NewGuid(),
                        DateTime.Now,
                        DateTime.Now,
                        0,
                        item.ProdutoId,
                        -item.Quantidade,
                        item.TamanhoId,
                        item.PesoId);
                }
                else
                {
                    estoque.UpdateEstoque(item.Quantidade, TipoMovimentacaoDeProduto.Saida);
                }

                estoques.Add(estoque);
                continue;
            }

            estoque.UpdateEstoque(item.Quantidade, TipoMovimentacaoDeProduto.Saida);
        }

        var estoquesAdd = estoques.Where(x => x.Numero == 0).ToList();
        var estoquesUpdate = estoques.Where(x => x.Numero > 0).ToList();

        if (estoquesUpdate.Count > 0)
        {
            _estoqueRepository.UpdateRange(estoquesUpdate);
        }

        if (estoquesAdd.Count > 0)
        {
            await _estoqueRepository.AddRangeAsync(estoquesAdd);
        }

        await _estoqueRepository.SaveChangesAsync();

        return true;
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
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro de estoque!");

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
