using ExpectedObjects;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
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
            Dominio = "https://localhost:3000",
            Ativo = true,
            ParceiroId = Guid.NewGuid()
        };

        var configuracao = new ConfiguracaoParceiro(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now,
            1,
            dto.ConexaoDb,
            dto.Dominio,
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
        Assert.Throws<ExceptionDomain>(
            () => ConfiguracaoParceiroBuilder
                    .Init()
                    .SemStringDb(conexaoDb)
                    .Build());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NaoDeveCriarDominio(string dominio)
    {
        Assert.Throws<ExceptionDomain>(
            () => ConfiguracaoParceiroBuilder
                    .Init()
                    .SemDominio(dominio)
                    .Build());
    }
}
