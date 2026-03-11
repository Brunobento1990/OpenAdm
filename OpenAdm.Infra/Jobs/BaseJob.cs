using Quartz;

namespace OpenAdm.Infra.Jobs;

public abstract class BaseJob : IJob
{
    public abstract Task ExecuteTask();
    public async Task Execute(IJobExecutionContext context)
    {
        await ExecuteTask();
    }
}