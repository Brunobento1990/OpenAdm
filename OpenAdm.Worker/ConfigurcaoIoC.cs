using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Worker.Application.HttpService.Interface;
using OpenAdm.Worker.Application.Interfaces;
using OpenAdm.Worker.Application.Service;
using OpenAdm.Worker.Infra.Enum;
using OpenAdm.Worker.Infra.HttpClient;
using OpenAdm.Worker.Infra.Repositories;
using OpenAdm.Worker.Jobs.Implementacoes;
using OpenAdm.Worker.Jobs.Interfaces;
using Quartz;

namespace OpenAdm.Worker;

public static class ConfigurcaoIoC
{
    public static IServiceCollection ConfigurarJobs(this IServiceCollection services, IConfiguration configuration)
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
                store.UsePostgres(configuration["ConnectionStrings:DefaultConnection"]!);
                store.UseSystemTextJsonSerializer();
                store.PerformSchemaValidation = false;
                store.UseProperties = true;
            });

            foreach (var job in jobs)
            {
                var key = (string)job.GetProperty("Key", BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!;
                var identityTrigger =
                    (string)job.GetProperty("IdentityTrigger", BindingFlags.Public | BindingFlags.Static)!
                        .GetValue(null)!;
                var cronMethod = job.GetMethod("ObterConfiguracaoTempoDeExecucao",
                    BindingFlags.Public | BindingFlags.Static)!;
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

    public static IServiceCollection InjectContext(this IServiceCollection services, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
        services.AddDbContext<ParceiroContext>(contextLifetime: ServiceLifetime.Scoped,
            optionsLifetime: ServiceLifetime.Scoped);

        services.AddScoped<IParceiroAutenticado, ParceiroAutenticadoWorker>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventoAplicacaoRepository, EventoAplicacaoRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IParceiroRepository, ParceiroRepository>();
        services.AddScoped<IConfiguracoesDePedidoRepository, ConfiguracoesDePedidoRepository>();

        return services;
    }

    public static IServiceCollection AddServicesApplication(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();

        services.AddKeyedScoped<IEventoAplicacaoService, NotificarNovoPedidoService>(TipoEventoAplicacaoEnum
            .EnviarPedidoWhatsApp);
        services.AddKeyedScoped<IEventoAplicacaoService, NotificarParceiroWhatsAppService>(TipoEventoAplicacaoEnum
            .NotificarParceiroWhatsApp);

        return services;
    }

    public static IServiceCollection AddHttpClientInfra(this IServiceCollection services, IConfiguration configuration)
    {
        var urlWhatsApp = configuration["WhatsApp:BaseUrl"]!;
        var apiKeyWhatsApp = configuration["WhatsApp:ApiKey"]!;

        services.AddHttpClient(nameof(HttpClientEnum.WhatsApp), x =>
        {
            x.BaseAddress = new Uri(urlWhatsApp);
            x.DefaultRequestHeaders.Add("apiKey", apiKeyWhatsApp);
        });

        services.AddScoped<IHttpClientWhatsApp, HttpClientWhatsApp>();

        return services;
    }
}