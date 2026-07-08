using OpenAdm.Worker.Application.Interfaces;
using StackExchange.Redis;

namespace OpenAdm.Worker.Infra.Services;

public class FilaService : IFilaService
{
    private readonly IConnectionMultiplexer _redis;

    public FilaService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<IFilaConsumer> InscreverAsync(string fila)
    {
        var consumer = new FilaConsumer(
            _redis.GetDatabase(),
            fila,
            grupo: fila,
            consumer: Environment.MachineName);

        await consumer.InicializarAsync();

        return consumer;
    }
}