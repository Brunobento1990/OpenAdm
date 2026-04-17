using Quartz;

namespace OpenAdm.Worker.Jobs.Implementacoes;

public abstract class BaseJob : IJob
{
    public abstract Task ExecuteTask();
    public async Task Execute(IJobExecutionContext context)
    {
        await ExecuteTask();
    }
}