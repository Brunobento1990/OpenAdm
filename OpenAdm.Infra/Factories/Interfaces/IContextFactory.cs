using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Factories.Interfaces;

public interface IContextFactory
{
    Task<ParceiroContext> GetParceiroContextAsync();
}
