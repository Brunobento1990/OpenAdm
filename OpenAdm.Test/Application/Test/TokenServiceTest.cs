using Microsoft.AspNetCore.Http;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Services;
using Domain.Pkg.Entities;

namespace OpenAdm.Test.Application.Test;

public class TokenServiceTest
{

    [Fact]
    public void DeveGerarUmToken()
    {
        ConfiguracaoDeToken.Configure("86c3fb1e-6b8b-42d0-922f-5c0fcd4b042c", "issue", "audience", 2);
        var funcionario = new Funcionario(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1, "email@gmail.com", "123", "Test", null, null, true);
        var tokenService = new TokenService();
        var token = tokenService.GenerateToken(funcionario);

        Assert.NotNull(token);
        Assert.True(!string.IsNullOrEmpty(token));
    }

}
