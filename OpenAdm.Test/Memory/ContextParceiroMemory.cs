using Domain.Pkg.Cryptography;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Test.Memory;

public static class ContextParceiroMemory
{
    public static ParceiroContext Build()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ParceiroContext>()
            .UseInMemoryDatabase("Open-Adm").Options;

        var configuracaoParceiroRepositoryMock = new Mock<IConfiguracaoParceiroRepository>();
        CryptographyGeneric.Configure("57b4c21f-6f41-4101-b6bd-62ad1cfca", "57b4c21f-6f41-41");

        configuracaoParceiroRepositoryMock.Setup(x => x.GetConexaoDbByDominioAsync()).ReturnsAsync("");
        var context = new ParceiroContext(optionsBuilder, configuracaoParceiroRepositoryMock.Object);
        
        return context;
    }
}
