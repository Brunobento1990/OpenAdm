using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Application.Models.Carrinhos;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class GetCarrinhoService : IGetCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IItensPedidoRepository _itensPedidoRepository;

    public GetCarrinhoService(
        ICarrinhoRepository carrinhoRepository,
        IProdutoRepository produtoRepository,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository,
        IUsuarioAutenticado usuarioAutenticado,
        IConfiguracoesDePedidoService configuracoesDePedidoService,
        IEstoqueRepository estoqueRepository, IItensPedidoRepository itensPedidoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
        _produtoRepository = produtoRepository;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _usuarioAutenticado = usuarioAutenticado;
        _configuracoesDePedidoService = configuracoesDePedidoService;
        _estoqueRepository = estoqueRepository;
        _itensPedidoRepository = itensPedidoRepository;
    }

    public async Task<CarrinhoViewModel> GetCarrinhoAsync()
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        var itensCarrinho = new List<ItemCarrinhoViewModel>();

        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_usuarioAutenticado.Id.ToString());
        var produtosIds = carrinho.Produtos.Select(x => x.ProdutoId).ToList();
        var itensTabelaDePreco = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);
        var produtos = await _produtoRepository.GetProdutosByListIdAsync(produtosIds);

        var config = await _configuracoesDePedidoService.ConfiguracaoDePedidoAsync();

        var estoques = await _estoqueRepository.GetPosicaoEstoqueDosProdutosAsync(produtosIds);
        var estoquesReservados = await _itensPedidoRepository.ObterEstoquesReservadosAsync(produtosIds);

        var estoquesTamanhos = estoques
            .Where(x => x.TamanhoId != null && x.PesoId == null)
            .ToDictionary(x => (x.ProdutoId, x.TamanhoId!.Value));

        var reservadosTamanhos = estoquesReservados
            .Where(x => x.TamanhoId != null && x.PesoId == null)
            .ToDictionary(x => (x.ProdutoId, x.TamanhoId!.Value));

        var estoquesPesos = estoques
            .Where(x => x.PesoId != null && x.TamanhoId == null)
            .ToDictionary(x => (x.ProdutoId, x.PesoId!.Value));

        var reservadosPesos = estoquesReservados
            .Where(x => x.PesoId != null && x.TamanhoId == null)
            .ToDictionary(x => (x.ProdutoId, x.PesoId!.Value));

        foreach (var produto in produtos)
        {
            var itemCarrinho = new ItemCarrinhoViewModel()
            {
                Categoria = new CategoriaViewModel().ToModel(produto.Categoria),
                CategoriaId = produto.CategoriaId,
                Descricao = produto.Descricao,
                EspecificacaoTecnica = produto.EspecificacaoTecnica,
                Id = produto.Id,
                Referencia = produto.Referencia,
                Numero = produto.Numero,
                Foto = produto.UrlFoto ?? ""
            };

            itemCarrinho.Tamanhos = produto
                .Tamanhos
                .OrderBy(x => x.Numero)
                .Select(x =>
                {
                    estoquesTamanhos.TryGetValue(
                        (produto.Id, x.Id),
                        out var estoque);

                    reservadosTamanhos.TryGetValue((produto.Id, x.Id), out var estoqueReservado);

                    var itemTabelaDePreco = itensTabelaDePreco
                        .FirstOrDefault(item => item.ProdutoId == produto.Id && item.TamanhoId == x.Id);

                    var quantidade = carrinho?
                        .Produtos?
                        .FirstOrDefault(pr => pr.ProdutoId == produto.Id && pr.TamanhoId == x.Id)?
                        .Quantidade ?? 0;

                    ProdutoEcommerceHelper
                        .AplicarEstoque(produto, config, estoque, estoqueReservado, out var estoqueDisponivel,
                            out var possuiEstoqueDisponivel);

                    return new TamanhoCarrinhoViewModel()
                    {
                        Id = x.Id,
                        Descricao = x.Descricao,
                        Numero = x.Numero,
                        Quantidade = estoqueDisponivel,
                        TemEstoqueDisponivel = possuiEstoqueDisponivel,
                        PrecoProduto = new QuantidadeProdutoCarrinhoViewModel()
                        {
                            Quantidade = quantidade,
                            ValorUnitario = ProdutoEcommerceHelper.ValorUnitarioProduto(
                                usuario.Cnpj,
                                itemTabelaDePreco?.ValorUnitarioAtacado,
                                itemTabelaDePreco?.ValorUnitarioVarejo
                            )
                        }
                    };
                }).ToList();

            itemCarrinho.Pesos = produto
                .Pesos
                .OrderBy(x => x.Numero)
                .Select(x =>
                {
                    estoquesPesos.TryGetValue(
                        (produto.Id, x.Id),
                        out var estoque);

                    reservadosPesos.TryGetValue((produto.Id, x.Id), out var estoqueReservado);

                    var itemTabelaDePreco = itensTabelaDePreco
                        .FirstOrDefault(item => item.ProdutoId == produto.Id && item.PesoId == x.Id);

                    var quantidade = carrinho?
                        .Produtos?
                        .FirstOrDefault(pr => pr.ProdutoId == produto.Id && pr.PesoId == x.Id)?
                        .Quantidade ?? 0;

                    ProdutoEcommerceHelper
                        .AplicarEstoque(produto, config, estoque, estoqueReservado, out var estoqueDisponivel,
                            out var possuiEstoqueDisponivel);

                    return new PesoCarrinhoViewModel()
                    {
                        Id = x.Id,
                        Descricao = x.Descricao,
                        Numero = x.Numero,
                        Quantidade = estoqueDisponivel,
                        TemEstoqueDisponivel = possuiEstoqueDisponivel,
                        PrecoProduto = new QuantidadeProdutoCarrinhoViewModel()
                        {
                            Quantidade = quantidade,
                            ValorUnitario = ProdutoEcommerceHelper.ValorUnitarioProduto(
                                usuario.Cnpj,
                                itemTabelaDePreco?.ValorUnitarioAtacado,
                                itemTabelaDePreco?.ValorUnitarioVarejo
                            )
                        }
                    };
                }).ToList();

            itensCarrinho.Add(itemCarrinho);
        }

        return new()
        {
            Itens = itensCarrinho,
            EnderecoUsuario = usuario.EnderecoUsuario == null
                ? null
                : new Models.Fretes.EnderecoViewModel()
                {
                    Bairro = usuario.EnderecoUsuario.Bairro,
                    Cep = usuario.EnderecoUsuario.Cep,
                    Complemento = usuario.EnderecoUsuario.Complemento,
                    Localidade = usuario.EnderecoUsuario.Localidade,
                    Logradouro = usuario.EnderecoUsuario.Logradouro,
                    Numero = usuario.EnderecoUsuario.Numero,
                    Uf = usuario.EnderecoUsuario.Uf
                }
        };
    }
}