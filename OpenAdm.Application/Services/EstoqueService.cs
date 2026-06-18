using System.ComponentModel.DataAnnotations;
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
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;

    public EstoqueService(
        IEstoqueRepository estoqueRepository,
        IMovimentacaoDeProdutoRepository movimentacaoDeProdutoRepository,
        IConfiguracoesDePedidoService configuracoesDePedidoService)
    {
        _estoqueRepository = estoqueRepository;
        _movimentacaoDeProdutoRepository = movimentacaoDeProdutoRepository;
        _configuracoesDePedidoService = configuracoesDePedidoService;
    }

    public async Task<EstoqueViewModel> GetEstoqueViewModelAsync(Guid id)
    {
        var estoque = await _estoqueRepository.GetEstoqueAsync(x => x.Id == id)
                      ?? throw new ExceptionApi("Não foi possível localizar o cadastro de estoque!");

        return (EstoqueViewModel)estoque;
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

    public async Task<TodosEstoquesDoProdutoViewModel> TodosEstoquesDoProdutoAsync(Guid produtoId)
    {
        var estoques = await _estoqueRepository.PosicaoEstoqueDoProdutoAsync(produtoId);

        var estoquesViewModel = estoques.Select(x => (EstoqueViewModel)x).ToList();

        return new()
        {
            Dados = estoquesViewModel
        };
    }

    public async Task AtualizarEstoquesAsync(UpdateEstoquesDto updateEstoqueDto)
    {
        var config = await _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();

        var estoques =
            await _estoqueRepository.GetPosicaoEstoqueAsync(updateEstoqueDto.Dados.Select(x => x.Id).ToList());

        foreach (var item in updateEstoqueDto.Dados)
        {
            if (!estoques.TryGetValue(item.Id, out var estoque))
            {
                continue;
            }

            var permitirEstoqueNegativo =
                !estoque.Produto.ExigeEstoqueDisponivel(config.VendaDeProdutoComEstoque);

            estoque.AjustarQuantidade(item.Quantidade ?? 0, permitirEstoqueNegativo);
            _estoqueRepository.Update(estoque);
        }

        await _estoqueRepository.SaveChangesAsync();
    }

    public async Task<bool> MovimentacaoDePedidoEntregueAsync(IList<ItemPedido> itens)
    {
        var config = await _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();

        var estoques = await _estoqueRepository
            .GetPosicaoEstoqueDosProdutosAsync(
                itens.Select(x => x.ProdutoId).Distinct().ToList());

        foreach (var item in itens)
        {
            var estoque = estoques.FirstOrDefault(x =>
                x.ProdutoId == item.ProdutoId &&
                x.PesoId == item.PesoId &&
                x.TamanhoId == item.TamanhoId);

            if (estoque == null)
            {
                throw new ValidationException(
                    $"Não foi localizado estoque para o produto {item.ProdutoId}.");
            }

            var permitirEstoqueNegativo =
                !estoque.Produto.ExigeEstoqueDisponivel(config.VendaDeProdutoComEstoque);

            estoque.AplicarMovimentacao(
                item.Quantidade,
                TipoMovimentacaoDeProduto.Saida,
                permitirEstoqueNegativo);
        }

        if (estoques.Count > 0)
        {
            _estoqueRepository.UpdateRange(estoques);
            await _estoqueRepository.SaveChangesAsync();
        }

        return true;
    }

    public async Task<PosicaoDeEstoqueRelatorioViewModel> PosicaoEstoqueRelatorioAsync(
        PosicaoDeEstoqueRelatorioDto posicaoDeEstoqueRelatorioDto)
    {
        var estoques = await _estoqueRepository.GetPosicaoEstoqueRelatorioAsync(
            produtos: posicaoDeEstoqueRelatorioDto.Produtos,
            pesos: posicaoDeEstoqueRelatorioDto.Pesos,
            tamanhos: posicaoDeEstoqueRelatorioDto.Tamanhos,
            categorias: posicaoDeEstoqueRelatorioDto.Categorias);

        var estoquesViewModel = new List<EstoqueViewModel>();

        foreach (var estoque in estoques)
        {
            estoquesViewModel.Add((EstoqueViewModel)estoque);
        }

        return new PosicaoDeEstoqueRelatorioViewModel()
        {
            Itens = estoquesViewModel
        };
    }

    public async Task<bool> MovimentacaoDeProdutoAsync(
        MovimentacaoDeProdutoDto dto)
    {
        var config = await _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();

        var estoque = await GetEstoqueLocalAsync(
            dto.ProdutoId,
            dto.PesoId,
            dto.TamanhoId);

        var date = DateTime.Now;

        if (estoque == null)
        {
            throw new ValidationException(
                "Não foi localizado estoque para este produto.");
        }

        var permitirEstoqueNegativo =
            !estoque.Produto.ExigeEstoqueDisponivel(
                config.VendaDeProdutoComEstoque);

        estoque.AplicarMovimentacao(
            dto.Quantidade,
            dto.TipoMovimentacaoDeProduto,
            permitirEstoqueNegativo);

        await _estoqueRepository.UpdateAsync(estoque);

        var movimento = new MovimentacaoDeProduto(
            Guid.NewGuid(),
            date,
            date,
            0,
            dto.Quantidade,
            dto.TipoMovimentacaoDeProduto,
            dto.ProdutoId,
            dto.TamanhoId,
            dto.PesoId,
            dto.Observacao);

        await _movimentacaoDeProdutoRepository.AddAsync(movimento);

        return true;
    }

    public async Task<bool> UpdateEstoqueAsync(UpdateEstoqueDto updateEstoqueDto)
    {
        var config = await _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();

        var estoque = await _estoqueRepository.GetEstoqueAsync(x =>
                          x.ProdutoId == updateEstoqueDto.ProdutoId &&
                          x.PesoId == updateEstoqueDto.PesoId &&
                          x.TamanhoId == updateEstoqueDto.TamanhoId)
                      ?? throw new ExceptionApi("Não foi possível localizar o cadastro de estoque!");

        var novaQuantidade = updateEstoqueDto.Quantidade ?? 0;

        var quantidadeMovimentada = Math.Abs(novaQuantidade - estoque.Quantidade);

        if (quantidadeMovimentada == 0)
        {
            return true;
        }

        var tipoMovimentacao = novaQuantidade > estoque.Quantidade
            ? TipoMovimentacaoDeProduto.Entrada
            : TipoMovimentacaoDeProduto.Saida;

        var permitirEstoqueNegativo =
            !estoque.Produto.ExigeEstoqueDisponivel(config.VendaDeProdutoComEstoque);

        estoque.AjustarQuantidade(
            novaQuantidade,
            permitirEstoqueNegativo);

        var date = DateTime.Now;

        var movimento = new MovimentacaoDeProduto(
            Guid.NewGuid(),
            date,
            date,
            0,
            quantidadeMovimentada,
            tipoMovimentacao,
            estoque.ProdutoId,
            estoque.TamanhoId,
            estoque.PesoId,
            $"Estoque ajustado manualmente: {estoque.Quantidade} → {novaQuantidade}");

        await _estoqueRepository.UpdateAsync(estoque);
        await _movimentacaoDeProdutoRepository.AddAsync(movimento);

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