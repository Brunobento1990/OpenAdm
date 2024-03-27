using Domain.Pkg.Entities;
using ExpectedObjects;
using Microsoft.AspNetCore.Http;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Services;
using OpenAdm.Application.Services.Carrinhos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;
using OpenAdm.Test.Domain.Builder;
using OpenAdm.Test.Memory;
using System.Security.Claims;

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
        var itemTabelaDePreco = new ItensTabelaDePreco(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, produto.Id, 1, 2, Guid.NewGuid(), tamanho.Id, null);
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
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var tokenService = new TokenService(httpContextAccessorMock.Object);
        var identity = new ClaimsIdentity(ConfiguracaoDeToken.GenerateClaimsFuncionario(usuario));
        httpContextAccessorMock.Setup(x => x.HttpContext.User.Identity).Returns(identity);
        produtoRepository.Setup(x => x.GetProdutosByListIdAsync(new List<Guid>() { produto.Id })).ReturnsAsync(new List<Produto>() { produto });
        itemTabelaDePrecoRepository.Setup(x => x.GetItensTabelaDePrecoByIdProdutosAsync(new List<Guid>() { produto.Id })).ReturnsAsync(new List<ItensTabelaDePreco>() { itemTabelaDePreco });
        var carrinhoService = new CarrinhoService(carrinhoRepository, produtoRepository.Object, tokenService, itemTabelaDePrecoRepository.Object);
        var carrinhoReturn = await carrinhoService.GetCarrinhoAsync();

        var produtoCarrinho = carrinhoReturn.FirstOrDefault(x => x.Id == produto.Id);

        Assert.NotNull(produtoCarrinho);
        Assert.Equal(produto.Id, produtoCarrinho.Id);
        Assert.Equal(quantidade, produtoCarrinho.Tamanhos?.FirstOrDefault(x => x.Id == tamanho.Id)?.PrecoProduto.Quantidade);
    }

    [Fact]
    public async void DeveAdicionarProdutoNoCarrinho()
    {
        ConfiguracaoDeToken.Configure("86c3fb1e-6b8b-42d0-922f-5c0fcd4b042c", "issue", "audience", 2);

        var usuario = UsuarioBuilder.Init().Build();
        var carrinho = new CarrinhoModel()
        {
            UsuarioId = usuario.Id,
            Produtos = new()
        };
        var tamanho = new Tamanho(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "Novo tamanho");
        var produto = ProdutoBuilder.Init().Build();
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var tokenService = new TokenService(httpContextAccessorMock.Object);
        var identity = new ClaimsIdentity(ConfiguracaoDeToken.GenerateClaimsFuncionario(usuario));
        var carrinhoRepository = new Mock<ICarrinhoRepository>();
        carrinhoRepository.Setup(x => x.GetCarrinhoAsync(usuario.Id.ToString())).ReturnsAsync(carrinho);

        httpContextAccessorMock.Setup(x => x.HttpContext.User.Identity).Returns(identity);

        var carrinhoService = new AddCarrinhoService(carrinhoRepository.Object, tokenService);

        var addCarrinho = new List<AddCarrinhoModel>()
        {
            new()
            {
                TamanhoId = tamanho.Id,
                Quantidade = 3,
                ProdutoId = produto.Id
            }
        };

        var result = await carrinhoService.AddCarrinhoAsync(addCarrinho);

        Assert.True(result);
    }
}
