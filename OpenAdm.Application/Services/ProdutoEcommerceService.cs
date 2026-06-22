using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Ecommerce;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class ProdutoEcommerceService : IProdutoEcommerceService
{
    private readonly IProdutoEcommerceRepository _repository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;
    private readonly IItensPedidoRepository _itensPedidoRepository;

    public ProdutoEcommerceService(IProdutoEcommerceRepository repository, IUsuarioAutenticado usuarioAutenticado,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository, IEstoqueRepository estoqueRepository,
        IConfiguracoesDePedidoService configuracoesDePedidoService, IItensPedidoRepository itensPedidoRepository)
    {
        _repository = repository;
        _usuarioAutenticado = usuarioAutenticado;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _estoqueRepository = estoqueRepository;
        _configuracoesDePedidoService = configuracoesDePedidoService;
        _itensPedidoRepository = itensPedidoRepository;
    }

    public async Task<ResultadoProdutoEcommerceModel> ListarAsync(ProdutoEcommerceFiltroDto filtro)
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoOrNullAsync();

        var resultado = await _repository.ListarAsync(
            filtro.Search,
            filtro.Page,
            filtro.CategoriasIds);

        var resultadoViewModel = new ResultadoProdutoEcommerceModel()
        {
            TotalPaginas = resultado.TotalPaginas,
        };

        var config = await _configuracoesDePedidoService.ConfiguracaoDePedidoAsync();

        var produtosIds = resultado.Values.Select(x => x.Id).ToList();

        var itensTabelaDePreco = await _itemTabelaDePrecoRepository
            .GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);
        var isAtacado = usuario?.IsAtacado == true;

        var estoques = await _estoqueRepository.GetPosicaoEstoqueDosProdutosAsync(produtosIds);

        var estoquesReservados = await _itensPedidoRepository.ObterEstoquesReservadosAsync(produtosIds);

        var precoPorTamanho = itensTabelaDePreco
            .Where(x => x.TamanhoId != null)
            .ToDictionary(
                x => (x.ProdutoId, x.TamanhoId),
                x => x
            );

        var precoPorPeso = itensTabelaDePreco
            .Where(x => x.PesoId != null)
            .ToDictionary(
                x => (x.ProdutoId, x.PesoId),
                x => x
            );

        foreach (var produto in resultado.Values)
        {
            var produtoViewModel = (ProdutoEcommerceModel)produto;

            if (filtro.PesosIds?.Count > 0)
            {
                produto.Pesos = produto.Pesos.Where(x => filtro.PesosIds.Contains(x.Id)).ToList();
            }

            if (filtro.TamanhosIds?.Count > 0)
            {
                produto.Tamanhos = produto.Tamanhos.Where(x => filtro.TamanhosIds.Contains(x.Id)).ToList();
            }

            foreach (var tamanho in produto.Tamanhos)
            {
                var tamanhoViewModel = new PesoTamanhoEcommerceModel()
                {
                    Descricao = tamanho.Descricao,
                    Id = tamanho.Id,
                };

                if (precoPorTamanho.TryGetValue((produto.Id, tamanho.Id), out var preco))
                {
                    tamanhoViewModel.ValorUnitario = isAtacado ? preco.ValorUnitarioAtacado : preco.ValorUnitarioVarejo;
                }

                var estoque = estoques.FirstOrDefault(x =>
                    x.ProdutoId == produto.Id &&
                    x.TamanhoId == tamanho.Id);

                var reservado = estoquesReservados.FirstOrDefault(x =>
                    x.ProdutoId == produto.Id &&
                    x.TamanhoId == tamanho.Id);

                ProdutoEcommerceHelper.AplicarEstoque(
                    produto,
                    config,
                    estoque,
                    reservado,
                    out var quantidadeDisponivel,
                    out var temEstoqueDisponivel);

                tamanhoViewModel.QuantidadeEstoqueDisponivel = quantidadeDisponivel;
                tamanhoViewModel.TemEstoqueDisponivel = temEstoqueDisponivel;

                produtoViewModel.Tamanhos.Add(tamanhoViewModel);
            }

            foreach (var peso in produto.Pesos)
            {
                var pesoViewModel = new PesoTamanhoEcommerceModel()
                {
                    Descricao = peso.Descricao,
                    Id = peso.Id,
                };

                if (precoPorPeso.TryGetValue((produto.Id, peso.Id), out var preco))
                {
                    pesoViewModel.ValorUnitario = isAtacado ? preco.ValorUnitarioAtacado : preco.ValorUnitarioVarejo;
                }
                
                var estoque = estoques.FirstOrDefault(x =>
                    x.ProdutoId == produto.Id &&
                    x.PesoId == peso.Id);

                var reservado = estoquesReservados.FirstOrDefault(x =>
                    x.ProdutoId == produto.Id &&
                    x.PesoId == peso.Id);

                ProdutoEcommerceHelper.AplicarEstoque(
                    produto,
                    config,
                    estoque,
                    reservado,
                    out var quantidadeDisponivel,
                    out var temEstoqueDisponivel);

                pesoViewModel.QuantidadeEstoqueDisponivel = quantidadeDisponivel;
                pesoViewModel.TemEstoqueDisponivel = temEstoqueDisponivel;

                produtoViewModel.Pesos.Add(pesoViewModel);
            }

            if (produtoViewModel.Pesos.Count > 0 || produtoViewModel.Tamanhos.Count > 0)
            {
                resultadoViewModel.Values.Add(produtoViewModel);
            }
        }

        return resultadoViewModel;
    }
}