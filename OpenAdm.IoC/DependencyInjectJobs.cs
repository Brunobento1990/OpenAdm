using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenAdm.Infra.Jobs;
using Quartz;

namespace OpenAdm.IoC;

public static class DependencyInjectJobs
{
    public static IServiceCollection ConfigurarJobs(this IServiceCollection services, IConfiguration configuration,
        string conectionString)
    {
        var jobs = typeof(BaseJob)
            .Assembly
            .GetTypes()
            .Where(type => !type.IsAbstract &&
                           typeof(BaseJob).IsAssignableFrom(type) &&
                           typeof(IJobInfo).IsAssignableFrom(type))
            .ToList();

        services.AddQuartz(q =>
        {
            q.UsePersistentStore(store =>
            {
                store.UsePostgres(conectionString);
                store.UseSystemTextJsonSerializer();
                store.PerformSchemaValidation = false;
                store.UseProperties = true;
            });

            foreach (var job in jobs)
            {
                var key = (string)job.GetProperty("Key", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!;
                var identityTrigger = (string)job.GetProperty("IdentityTrigger", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!;
                var cronMethod = job.GetMethod("ObterConfiguracaoTempoDeExecucao", BindingFlags.Public | BindingFlags.Static)!;
                var cronSchedule = (string)cronMethod.Invoke(null, [configuration])!;

                var jobKey = new JobKey(key);

                q.AddJob(
                    job,
                    jobKey,
                    opts => opts.StoreDurably());

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity(identityTrigger)
                    .WithCronSchedule(cronSchedule));
            }
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}