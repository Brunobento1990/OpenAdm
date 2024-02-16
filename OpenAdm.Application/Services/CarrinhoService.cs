using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Carrinhos;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;
using System.Text;

namespace OpenAdm.Application.Services;

public class CarrinhoService : ICarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly string _key;

    public CarrinhoService(ICarrinhoRepository carrinhoRepository, IProdutoRepository produtoRepository, ITokenService tokenService)
    {
        _carrinhoRepository = carrinhoRepository;
        _produtoRepository = produtoRepository;
        _key = tokenService.GetTokenUsuarioViewModel().Id.ToString();
    }

    public async Task<bool> AdicionarProdutoAsync(AddCarrinhoModel addCarrinhoModel)
    {
        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_key);

        if(carrinho.UsuarioId == Guid.Empty)
            carrinho.UsuarioId = Guid.Parse(_key);
        
        var addProduto = carrinho?
            .Produtos
            .FirstOrDefault(x => x.ProdutoId == addCarrinhoModel.ProdutoId);

        if (addProduto == null)
        {
            addProduto = new()
            {
                ProdutoId = addCarrinhoModel.ProdutoId,
                Pesos = addCarrinhoModel.Pesos,
                Tamanhos = addCarrinhoModel.Tamanhos
            };

            carrinho?.Produtos.Add(addProduto);
        }
        else
        {
            AddPesosCarrinho(addCarrinhoModel.Pesos, addProduto);
            AddTamanhosCarrinho(addCarrinhoModel.Tamanhos, addProduto);
        }

        await _carrinhoRepository.AdicionarProdutoAsync(carrinho, _key);

        return true;
    }

    public async Task<bool> DeleteProdutoCarrinhoAsync(Guid produtoId)
    {
        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_key);

        if (carrinho.UsuarioId == Guid.Empty)
            carrinho.UsuarioId = Guid.Parse(_key);

        var produto = carrinho.Produtos.FirstOrDefault(x => x.ProdutoId == produtoId);

        if (produto != null)
        {
            carrinho.Produtos.Remove(produto);
            await _carrinhoRepository.AdicionarProdutoAsync(carrinho, _key);
        }

        return true;
    }

    public async Task<IList<CarrinhoViewModel>> GetCarrinhoAsync()
    {
        var carrinhosViewModels = new List<CarrinhoViewModel>();

        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_key);

        var produtosIds = carrinho.Produtos.Select(x => x.ProdutoId).ToList();

        var produtos = await _produtoRepository.GetProdutosByListIdAsync(produtosIds);

        foreach (var produto in produtos)
        {
            var carrinhoViewModel = new CarrinhoViewModel()
            {
                Categoria = new CategoriaViewModel().ToModel(produto.Categoria),
                CategoriaId = produto.CategoriaId,
                Descricao = produto.Descricao,
                EspecificacaoTecnica = produto.EspecificacaoTecnica,
                Foto = Encoding.UTF8.GetString(produto.Foto),
                Id = produto.Id,
                Referencia = produto.Referencia,
                Numero = produto.Numero
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
                        .FirstOrDefault(pr => pr.ProdutoId == produto.Id)?
                            .Tamanhos.FirstOrDefault(ps => ps.TamanhoId == x.Id)?
                                .Quantidade ?? 0)
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
                        .FirstOrDefault(pr => pr.ProdutoId == produto.Id)?
                            .Pesos.FirstOrDefault(ps => ps.PesoId == x.Id)?
                                .Quantidade ?? 0)
                }
            }).ToList();

            carrinhosViewModels.Add(carrinhoViewModel);
        }

        return carrinhosViewModels;
    }

    public async Task<int> GetCountCarrinhoAsync()
    {
        return await _carrinhoRepository.GetCountCarrinhoAsync(_key);
    }

    private static void AddPesosCarrinho(List<AddPesoCarrinho> pesos, AddCarrinhoModel addProduto)
    {
        foreach (var pesoCarrinho in pesos)
        {
            var peso = addProduto
                .Pesos
                .FirstOrDefault(ps => ps.PesoId == pesoCarrinho.PesoId);

            if (peso == null)
            {
                peso = new()
                {
                    PesoId = pesoCarrinho.PesoId,
                    Quantidade = pesoCarrinho.Quantidade
                };

                addProduto.Pesos.Add(peso);
            }
            else
            {
                peso.Quantidade += pesoCarrinho.Quantidade;
            }
        }
    }
    private static void AddTamanhosCarrinho(List<AddTamanhoCarrinho> tamanhos, AddCarrinhoModel addProduto)
    {
        foreach (var tamanhoCarrinho in tamanhos)
        {
            var tamanho = addProduto.Tamanhos.FirstOrDefault(tm => tm.TamanhoId == tamanhoCarrinho.TamanhoId);

            if (tamanho == null)
            {
                tamanho = new()
                {
                    TamanhoId = tamanhoCarrinho.TamanhoId,
                    Quantidade = tamanhoCarrinho.Quantidade
                };

                addProduto.Tamanhos.Add(tamanho);
            }
            else
            {
                tamanho.Quantidade += tamanhoCarrinho.Quantidade;
            }
        }
    }
}
