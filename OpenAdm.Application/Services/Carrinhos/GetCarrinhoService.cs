using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Application.Models.Carrinhos;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class GetCarrinhoService : IGetCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;

    public GetCarrinhoService(
        ICarrinhoRepository carrinhoRepository, 
        IProdutoRepository produtoRepository, 
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
        _produtoRepository = produtoRepository;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
    }

    public async Task<IList<CarrinhoViewModel>> GetCarrinhoAsync(UsuarioViewModel usuarioViewModel)
    {
        var carrinhosViewModels = new List<CarrinhoViewModel>();

        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(usuarioViewModel.Id.ToString());

        var produtosIds = carrinho.Produtos.Select(x => x.ProdutoId).ToList();

        var itensTabelaDePreco = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);
        var produtos = await _produtoRepository.GetProdutosByListIdAsync(produtosIds);

        foreach (var produto in produtos)
        {
            var carrinhoViewModel = new CarrinhoViewModel()
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

            carrinhoViewModel.Tamanhos = produto.Tamanhos.OrderBy(x => x.Numero).Select(x => new TamanhoCarrinhoViewModel()
            {
                Id = x.Id,
                Descricao = x.Descricao,
                Numero = x.Numero,
                PrecoProduto = new QuantidadeProdutoCarrinhoViewModel()
                {
                    Quantidade = (decimal)(carrinho?
                        .Produtos?
                        .FirstOrDefault(pr => pr.ProdutoId == produto.Id && pr.TamanhoId == x.Id)?
                                .Quantidade ?? 0),
                    ValorUnitario = GetValorUnitarioProduto(
                        usuarioViewModel.Cnpj,
                        itensTabelaDePreco
                        .FirstOrDefault(item => item.ProdutoId == produto.Id && item.TamanhoId == x.Id)?.ValorUnitarioAtacado,
                        itensTabelaDePreco
                        .FirstOrDefault(item => item.TamanhoId == x.Id && item.ProdutoId == produto.Id)?.ValorUnitarioVarejo)
                }
            }).ToList();

            carrinhoViewModel.Pesos = produto.Pesos.OrderBy(x => x.Numero).Select(x => new PesoCarrinhoViewModel()
            {
                Id = x.Id,
                Descricao = x.Descricao,
                Numero = x.Numero,
                PrecoProduto = new QuantidadeProdutoCarrinhoViewModel()
                {
                    Quantidade = (decimal)(carrinho?
                        .Produtos?
                        .FirstOrDefault(pr => pr.ProdutoId == produto.Id && pr.PesoId == x.Id)?
                                .Quantidade ?? 0),
                    ValorUnitario = GetValorUnitarioProduto(
                        usuarioViewModel.Cnpj,
                        itensTabelaDePreco
                        .FirstOrDefault(item => item.ProdutoId == produto.Id && item.PesoId == x.Id)?.ValorUnitarioAtacado,
                        itensTabelaDePreco
                        .FirstOrDefault(item => item.PesoId == x.Id && item.ProdutoId == produto.Id)?.ValorUnitarioVarejo
                        )
                }
            }).ToList();

            carrinhosViewModels.Add(carrinhoViewModel);
        }

        return carrinhosViewModels;
    }

    private static decimal GetValorUnitarioProduto(string? cnpj, decimal? valorUnitarioAtacado, decimal? valorUnitarioVarejo)
    {
        var isAtacado = !string.IsNullOrWhiteSpace(cnpj);

        if (isAtacado)
        {
            return valorUnitarioAtacado ?? 0;
        }

        return valorUnitarioVarejo ?? 0;
    }
}
