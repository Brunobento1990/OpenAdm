using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Application.Models.Carrinhos;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class GetCarrinhoService : IGetCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IItemTabelaDePrecoRepository _itemTabelaDePrecoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    public GetCarrinhoService(
        ICarrinhoRepository carrinhoRepository,
        IProdutoRepository produtoRepository,
        IItemTabelaDePrecoRepository itemTabelaDePrecoRepository,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _carrinhoRepository = carrinhoRepository;
        _produtoRepository = produtoRepository;
        _itemTabelaDePrecoRepository = itemTabelaDePrecoRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<CarrinhoViewModel> GetCarrinhoAsync()
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        var itensCarrinho = new List<ItemCarrinhoViewModel>();

        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_usuarioAutenticado.Id.ToString());
        var produtosIds = carrinho.Produtos.Select(x => x.ProdutoId).ToList();
        var itensTabelaDePreco = await _itemTabelaDePrecoRepository.GetItensTabelaDePrecoByIdProdutosAsync(produtosIds);
        var produtos = await _produtoRepository.GetProdutosByListIdAsync(produtosIds);

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
                .Select(x => new TamanhoCarrinhoViewModel()
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
                            usuario.Cnpj,
                            itensTabelaDePreco
                            .FirstOrDefault(item => item.ProdutoId == produto.Id && item.TamanhoId == x.Id)?.ValorUnitarioAtacado,
                            itensTabelaDePreco
                            .FirstOrDefault(item => item.TamanhoId == x.Id && item.ProdutoId == produto.Id)?.ValorUnitarioVarejo)
                    }
                }).ToList();

            itemCarrinho.Pesos = produto
                .Pesos
                .OrderBy(x => x.Numero)
                .Select(x => new PesoCarrinhoViewModel()
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
                            usuario.Cnpj,
                            itensTabelaDePreco
                            .FirstOrDefault(item => item.ProdutoId == produto.Id && item.PesoId == x.Id)?.ValorUnitarioAtacado,
                            itensTabelaDePreco
                            .FirstOrDefault(item => item.PesoId == x.Id && item.ProdutoId == produto.Id)?.ValorUnitarioVarejo
                            )
                    }
                }).ToList();

            itensCarrinho.Add(itemCarrinho);
        }

        return new()
        {
            Itens = itensCarrinho,
            EnderecoUsuario = usuario.EnderecoUsuario == null ? null : new Models.Fretes.EnderecoViewModel()
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

    private static decimal GetValorUnitarioProduto(
        string? cnpj, 
        decimal? valorUnitarioAtacado, 
        decimal? valorUnitarioVarejo)
    {
        var isAtacado = !string.IsNullOrWhiteSpace(cnpj);

        if (isAtacado)
        {
            return valorUnitarioAtacado ?? 0;
        }

        return valorUnitarioVarejo ?? 0;
    }
}
