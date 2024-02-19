using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using ExpectedObjects;
using OpenAdm.Test.Domain.Builder;

namespace OpenAdm.Test.Domain.Test;

public class ConfiguracaoParceiroTest
{
    [Fact]
    public void DeveCriarConfiguracaoParceiro()
    {
        var dto = new
        {
            ConexaoDb = "postgres",
            DominioSiteAdm = "https://localhost:3000",
            DominioSiteEcommerce = "https://localhost:3001",
            Ativo = true,
            ParceiroId = Guid.NewGuid()
        };

        var configuracao = new ConfiguracaoParceiro(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now,
            1,
            dto.ConexaoDb,
            dto.DominioSiteAdm,
            dto.DominioSiteEcommerce,
            dto.Ativo,
            dto.ParceiroId);

        dto
          .ToExpectedObject()
          .ShouldMatch(configuracao);

    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarSemConexaoDb(string conexaoDb)
    {
        Assert.Throws<ExceptionApi>(
            () => ConfiguracaoParceiroBuilder
                    .Init()
                    .SemStringDb(conexaoDb)
                    .Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarSemDominioSiteAdm(string dominio)
    {
        Assert.Throws<ExceptionApi>(
            () => ConfiguracaoParceiroBuilder
                    .Init()
                    .SemDominioSiteAdm(dominio)
                    .Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarSemDominioSiteEcommerce(string dominio)
    {
        Assert.Throws<ExceptionApi>(
            () => ConfiguracaoParceiroBuilder
                    .Init()
                    .SemDominioSiteEcommerce(dominio)
                    .Build());
    }
}
