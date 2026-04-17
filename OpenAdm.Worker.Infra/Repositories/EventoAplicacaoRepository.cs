using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Worker.Infra.Repositories;

public class EventoAplicacaoRepository : IEventoAplicacaoRepository
{
    private readonly AppDbContext _context;

    public EventoAplicacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(EventoAplicacao eventoAplicacao)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<EventoAplicacao> eventoAplicacao)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<EventoAplicacao>> ProximosEventosAsync()
    {
        return await _context
            .EventosAplicacao
            .Where(x => !x.Finalizado && x.QuantidadeDeTentativa < 3)
            .ToListAsync();
    }
}