using Microsoft.Extensions.Configuration;

namespace OpenAdm.Infra.Jobs;

public interface IJobInfo
{
    static abstract string Key { get; }
    static abstract string IdentityTrigger { get; }
    static abstract string ObterConfiguracaoTempoDeExecucao(IConfiguration configuracao);
}
