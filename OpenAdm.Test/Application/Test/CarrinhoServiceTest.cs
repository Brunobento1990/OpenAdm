using Domain.Pkg.Entities;
using Microsoft.AspNetCore.Http;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;
using OpenAdm.Test.Domain.Builder;
using System.Security.Claims;

namespace OpenAdm.Test.Application.Test;

public class CarrinhoServiceTest
{
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
        var produtoRepository = new Mock<IProdutoRepository>();
        var itensTabelaDePrecoRepository = new Mock<IItemTabelaDePrecoRepository>();
        carrinhoRepository.Setup(x => x.GetCarrinhoAsync(usuario.Id.ToString())).ReturnsAsync(carrinho);

        httpContextAccessorMock.Setup(x => x.HttpContext.User.Identity).Returns(identity);

        var carrinhoService = new CarrinhoService(carrinhoRepository.Object, produtoRepository.Object, tokenService, itensTabelaDePrecoRepository.Object);

        var addCarrinho = new AddCarrinhoModel()
        {
            ProdutoId = produto.Id,
            Tamanhos =
            [
                new()
                {
                    TamanhoId = tamanho.Id,
                    Quantidade = 3
                }
            ]
        };

        var result = await carrinhoService.AdicionarProdutoAsync(addCarrinho);

        Assert.True(result);
    }
}
