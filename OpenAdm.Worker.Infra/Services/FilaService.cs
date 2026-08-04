using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Model.Eventos;
using OpenAdm.Worker.Application.Interfaces;
using StackExchange.Redis;

namespace OpenAdm.Worker.Infra.Services;

public class FilaService : IFilaService
{
    private readonly IDatabase _db;

    public FilaService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<IFilaConsumer> InscreverAsync(string fila)
    {
        var consumer = new FilaConsumer(
            _db,
            fila,
            grupo: fila,
            consumer: Environment.MachineName);

        await consumer.InicializarAsync();

        return consumer;
    }

    public async Task PublicarAsync(EventoAplicacao evento)
    {
        await _db.StreamAddAsync(
            EventoBase.FilaEventoAplicacao,
            new NameValueEntry[]
            {
                new("data", evento.ToString())
            });
    }
}
