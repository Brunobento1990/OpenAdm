using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Model.Eventos;
using StackExchange.Redis;

namespace OpenAdm.Infra.Repositories;

public class FilaRepository : IFilaService
{
    private readonly IDatabase _db;

    public FilaRepository(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
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