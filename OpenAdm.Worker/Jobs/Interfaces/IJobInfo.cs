namespace OpenAdm.Worker.Jobs.Interfaces;

public interface IJobInfo
{
    static abstract string Key { get; }
    static abstract string IdentityTrigger { get; }
    static abstract string ObterConfiguracaoTempoDeExecucao(IConfiguration configuracao);
}