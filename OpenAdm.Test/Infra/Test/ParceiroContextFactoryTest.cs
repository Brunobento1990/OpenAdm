using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Factories.Factory;
using OpenAdm.Infra.Factories.Interfaces;

namespace OpenAdm.Test.Infra.Test;

public class ParceiroContextFactoryTest
{
    [Fact]
    public async Task NaoDeveCriarContextSemConexao()
    {
        var domainFactoryMock = new Mock<IDomainFactory>();
        var configuracaoParceiroRepositoryMock = new Mock<IConfiguracaoParceiroRepository>();

        var parceiroContext = new ParceiroContextFactory(domainFactoryMock.Object, configuracaoParceiroRepositoryMock.Object);

        ExceptionApi? exception = null;
        try
        {
            await parceiroContext.CreateParceiroContextAsync();
        }
        catch (ExceptionApi ex)
        {
            exception = ex;
        }

        Assert.IsType<ExceptionApi>(exception);
        Assert.NotNull(exception);
        Assert.Equal(ContextErrorMessage.StringDeConexaoInvalida, exception.Message);
    }

    [Fact]
    public async Task DeveCriarContextDoParceiro()
    {
        var dominio = "http://localhost:8475";
        var stringConection = "conexao db";

        var domainFactoryMock = new Mock<IDomainFactory>();
        domainFactoryMock.Setup(x => x.GetDomainParceiro())
            .Returns(dominio);

        var configuracaoParceiroRepositoryMock = new Mock<IConfiguracaoParceiroRepository>();
        configuracaoParceiroRepositoryMock.Setup(x => x.GetConexaoDbByDominioAsync(It.IsAny<string>()))
            .ReturnsAsync(stringConection);

        var factory = new ParceiroContextFactory(domainFactoryMock.Object, configuracaoParceiroRepositoryMock.Object);

        var context = await factory.CreateParceiroContextAsync();

        Assert.NotNull(context);
        Assert.IsType<ParceiroContext>(context);
    }
}
