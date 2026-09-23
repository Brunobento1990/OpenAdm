using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Services;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Test.Application.Test;

public class TokenServiceTest
{
    [Fact]
    public void DeveGerarUmToken()
    {
        ConfiguracaoDeToken.Configure("86c3fb1e-6b8b-42d0-922f-5c0fcd4b042c", "issue", "audience", 2, "");
        var sessao = SessaoUsuario.Criar(
            Guid.NewGuid(), Guid.NewGuid(), true, 10, new UsuarioSessaoRequest());
        var tokenService = new TokenService();
        var token = tokenService.GenerateToken(sessao);

        Assert.NotNull(token);
        Assert.True(!string.IsNullOrEmpty(token));

        var resultado = tokenService.ValidarToken(token);
        Assert.Equal(sessao.Id, resultado.Result?.SessaoId);
        Assert.Equal(sessao.UsuarioId, resultado.Result?.Id);
        Assert.Equal(sessao.ParceiroId, resultado.Result?.ParceiroId);
        Assert.True(resultado.Result?.EhFuncionario);
    }
}
