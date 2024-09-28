using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Application.Services.Carrinhos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;
using OpenAdm.Test.Domain.Builder;
using OpenAdm.Test.Memory;

namespace OpenAdm.Test.Application.Test;

public class CarrinhoServiceTest
{
    [Fact]
    public async Task DeveRetornarProdutosDoCarrinhoCorretos()
    {
        var quantidade = 12;
        var categoria = CategoriaBuilder.Init().Build();
        var carrinhoRepository = new CarrinhoRepositoryMemory();
        var tamanho = new Tamanho(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "Novo tamanho");
        var produto = ProdutoBuilder.Init().Build();
        produto.Categoria = categoria;
        produto.Tamanhos.Add(tamanho);
        var itemTabelaDePreco = new ItemTabelaDePreco(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, produto.Id, 1, 2, Guid.NewGuid(), tamanho.Id, null);
        var usuario = UsuarioBuilder.Init().Build();
        var carrinho = new CarrinhoModel()
        {
            UsuarioId = usuario.Id,
            Produtos = new()
            {
                new AddCarrinhoModel()
                {
                    ProdutoId = produto.Id,
                    TamanhoId = tamanho.Id,
                    Quantidade = quantidade
                }
            }
        };

        await carrinhoRepository.AdicionarProdutoAsync(carrinho, usuario.Id.ToString());

        var produtoRepository = new Mock<IProdutoRepository>();
        var itemTabelaDePrecoRepository = new Mock<IItemTabelaDePrecoRepository>();
        produtoRepository.Setup(x => x.GetProdutosByListIdAsync(new List<Guid>() { produto.Id })).ReturnsAsync(new List<Produto>() { produto });
        itemTabelaDePrecoRepository.Setup(x => x.GetItensTabelaDePrecoByIdProdutosAsync(new List<Guid>() { produto.Id })).ReturnsAsync(new List<ItemTabelaDePreco>() { itemTabelaDePreco });
        var carrinhoService = new GetCarrinhoService(carrinhoRepository, produtoRepository.Object, itemTabelaDePrecoRepository.Object);
        var carrinhoReturn = await carrinhoService.GetCarrinhoAsync(new UsuarioViewModel().ToModel(usuario));

        var produtoCarrinho = carrinhoReturn.FirstOrDefault(x => x.Id == produto.Id);

        Assert.NotNull(produtoCarrinho);
        Assert.Equal(produto.Id, produtoCarrinho.Id);
        Assert.Equal(quantidade, produtoCarrinho.Tamanhos?.FirstOrDefault(x => x.Id == tamanho.Id)?.PrecoProduto.Quantidade);
    }

    [Fact]
    public async Task DeveAdicionarProdutoNoCarrinho()
    {
        var usuario = UsuarioBuilder.Init().Build();
        var tamanho = new Tamanho(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "Novo tamanho");
        var produto = ProdutoBuilder.Init().Build();
        var addCarrinho = new List<AddCarrinhoModel>()
        {
            new()
            {
                TamanhoId = tamanho.Id,
                Quantidade = 3,
                ProdutoId = produto.Id
            }
        };
        var carrinho = new CarrinhoModel()
        {
            UsuarioId = usuario.Id,
            Produtos = addCarrinho
        };

        var carrinhoRepository = new CarrinhoRepositoryMemory();
        var carrinhoService = new AddCarrinhoService(carrinhoRepository);
        var result = await carrinhoService.AddCarrinhoAsync(addCarrinho, new UsuarioViewModel().ToModel(usuario));

        var carrinhoCount = new GetCountCarrinhoService(carrinhoRepository);
        var count = await carrinhoCount.GetCountCarrinhoAsync(usuario.Id);

        Assert.Equal(carrinho.Produtos.Count, count);
        Assert.True(result);
    }

    [Fact]
    public async Task DeveDeletarProdutoDoCarrinho()
    {
        var usuario = UsuarioBuilder.Init().Build();
        var tamanho = new Tamanho(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "Novo tamanho");
        var produto = ProdutoBuilder.Init().Build();
        var addCarrinho = new List<AddCarrinhoModel>()
        {
            new()
            {
                TamanhoId = tamanho.Id,
                Quantidade = 3,
                ProdutoId = produto.Id
            }
        };

        var carrinhoRepository = new CarrinhoRepositoryMemory();
        var carrinhoService = new AddCarrinhoService(carrinhoRepository);
        var result = await carrinhoService.AddCarrinhoAsync(addCarrinho, new UsuarioViewModel().ToModel(usuario));

        var deleteCarrinho = new DeleteProdutoCarrinhoService(carrinhoRepository);
        var resultDelete = await deleteCarrinho.DeleteProdutoCarrinhoAsync(produto.Id, usuario.Id);

        Assert.True(resultDelete);
        Assert.True(result);
    }
}
