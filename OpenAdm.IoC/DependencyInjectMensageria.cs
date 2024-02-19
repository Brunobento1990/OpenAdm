using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Mensageria;
using OpenAdm.Application.Mensageria.Interfaces;
using OpenAdm.Application.Mensageria.Producer;
using RabbitMQ.Client;

namespace OpenAdm.IoC;

public static class DependencyInjectMensageria
{
    public static void InjectMensageria(this IServiceCollection services, string url)
    {
        services.AddSingleton<IConnection>(s => ConfiguracaoBase.InitConnection(url));
        services.AddTransient<IModel>(s => s.GetRequiredService<IConnection>().CreateModel());
        services.AddScoped(typeof(IProducerGeneric<>), typeof(ProducerGeneric<>));
    }
}
