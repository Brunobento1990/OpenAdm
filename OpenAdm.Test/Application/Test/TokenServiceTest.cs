using Microsoft.AspNetCore.Http;
using OpenAdm.Application.Models.Funcionarios;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Services;
using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using System.Security.Claims;

namespace OpenAdm.Test.Application.Test;

public class TokenServiceTest
{

    [Fact]
    public void DeveGerarUmToken()
    {
        ConfiguracaoDeToken.Configure("86c3fb1e-6b8b-42d0-922f-5c0fcd4b042c", "issue", "audience", 2);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var funcionario = new Funcionario(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "email@gmail.com", "123", "Test", null, null, true);
        var tokenService = new TokenService(httpContextAccessorMock.Object);
        var token = tokenService.GenerateToken(funcionario);

        Assert.NotNull(token);
        Assert.True(!string.IsNullOrEmpty(token));
    }

    [Fact]
    public void DeveRetornarIsFuncionario()
    {
        ConfiguracaoDeToken.Configure("86c3fb1e-6b8b-42d0-922f-5c0fcd4b042c", "issue", "audience", 2);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var funcionario = new FuncionarioViewModel()
        {
            DataDeAtualizacao = DateTime.Now,
            DataDeCriacao = DateTime.Now,
            Email = "teste@gmail.com",
            Id = Guid.NewGuid(),
            Nome = "Testes",
            Numero = 1
        };
        var tokenService = new TokenService(httpContextAccessorMock.Object);
        var identity = new ClaimsIdentity(TokenService.GenerateClaims(funcionario));
        httpContextAccessorMock.Setup(x => x.HttpContext.User.Identity).Returns(identity);

        var isFuncionario = tokenService.IsFuncionario();

        Assert.True(isFuncionario);
    }

    [Fact]
    public void DeveRetornarExceptionApiIsFuncionario()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var tokenService = new TokenService(httpContextAccessorMock.Object);

        Assert.Throws<ExceptionApi>(() => tokenService.IsFuncionario());
    }
}
