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

    public async Task AddAsync(EventoAplicacao eventoAplicacao)
    {
        await _context.EventosAplicacao.AddAsync(eventoAplicacao);
    }

    public Task AddRangeAsync(IEnumerable<EventoAplicacao> eventoAplicacao)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<EventoAplicacao?> ProximoEventoAsync()
    {
        return await _context
            .EventosAplicacao
            .Include(x => x.EmpresaOpenAdm)
            .OrderBy(x => x.DataDeCriacao)
            .FirstOrDefaultAsync(x => !x.Finalizado && x.QuantidadeDeTentativa < 3);
    }
}