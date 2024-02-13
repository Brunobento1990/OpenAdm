using Microsoft.AspNetCore.Http;
using OpenAdm.Application.Models;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using System.Security.Claims;

namespace OpenAdm.Test.Application.Test;

public class TokenServiceTest
{
    private readonly ConfiguracaoDeToken _configtoken =
        new("11782be2-2f69-4bc9-95b2-6649c726260a", "issue", "audience", DateTime.Now.AddHours(24));

    [Fact]
    public void DeveGerarUmToken()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var funcionario = new Funcionario(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "email@gmail.com", "123", "Test", null, null);
        var tokenService = new TokenService(httpContextAccessorMock.Object);
        var token = tokenService.GenerateToken(funcionario, _configtoken);

        Assert.NotNull(token);
        Assert.True(!string.IsNullOrEmpty(token));
    }

    [Fact]
    public void DeveRetornarIsFuncionario()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var funcionario = new Funcionario(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "email@gmail.com", "123", "Test", null, null);
        var tokenService = new TokenService(httpContextAccessorMock.Object);
        var identity = new ClaimsIdentity(_configtoken.GenerateClaimsFuncionario(funcionario));
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
