using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Domain.Interfaces;

public interface IEventoAplicacaoRepository
{
    Task AddAsync(EventoAplicacao eventoAplicacao);
    Task AddRangeAsync(IEnumerable<EventoAplicacao> eventoAplicacao);
    Task SaveChangesAsync();
    Task<ICollection<EventoAplicacao>> ProximosEventosAsync();
}