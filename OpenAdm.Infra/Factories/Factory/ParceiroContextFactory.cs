using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Factories.Interfaces;

namespace OpenAdm.Infra.Factories.Factory;

public class ParceiroContextFactory : IParceiroContextFactory
{
    private readonly string _dominio;
    private readonly IConfiguracaoParceiroRepository _configuracaoParceiroRepository;

    public ParceiroContextFactory(
        IDomainFactory domainFactory,
        IConfiguracaoParceiroRepository configuracaoParceiroRepository)
    {
        _dominio = domainFactory.GetDomainParceiro();
        _configuracaoParceiroRepository = configuracaoParceiroRepository;
    }

    public async Task<ParceiroContext> CreateParceiroContextAsync()
    {
        var conexao = await _configuracaoParceiroRepository
            .GetConexaoDbByDominioAsync(_dominio) ??
            throw new ExceptionDomain(ContextErrorMessage.StringDeConexaoInvalida);

        return CreateContext(conexao);
    }

    private static ParceiroContext CreateContext(string conexao)
    {
        var optionsBuilderClient = new DbContextOptionsBuilder<ParceiroContext>();

        optionsBuilderClient.UseNpgsql(conexao,
            b => b.MigrationsAssembly(typeof(ParceiroContext).Assembly.FullName));

        return new ParceiroContext(optionsBuilderClient.Options);
    }
}
