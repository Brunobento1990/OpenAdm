using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Factories.Interfaces;

public interface IParceiroContextFactory
{
    Task<ParceiroContext> CreateParceiroContextAsync();
}
