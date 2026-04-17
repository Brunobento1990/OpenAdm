using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class EventoAplicacaoRepository : IEventoAplicacaoRepository
{
    private readonly AppDbContext _appDbContext;

    public EventoAplicacaoRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(EventoAplicacao eventoAplicacao)
    {
        await _appDbContext.EventosAplicacao.AddAsync(eventoAplicacao);
    }

    public async Task AddRangeAsync(IEnumerable<EventoAplicacao> eventoAplicacao)
    {
        await _appDbContext.EventosAplicacao.AddRangeAsync(eventoAplicacao);
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public Task<ICollection<EventoAplicacao>> ProximosEventosAsync()
    {
        throw new NotImplementedException();
    }
}