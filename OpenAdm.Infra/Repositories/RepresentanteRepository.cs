using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public sealed class RepresentanteRepository(ParceiroContext parceiroContext)
    : GenericRepository<Representante>(parceiroContext), IRepresentanteRepository
{
    public Task<Representante?> ObterPorIdAsync(Guid id, bool tracking = false)
    {
        var query = ParceiroContext.Representantes.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<(bool CpfDuplicado, bool EmailDuplicado)> ObterDuplicidadesAsync(
        string? cpf, string? email, Guid? ignorarId = null)
    {
        var duplicidades = await ParceiroContext.Representantes
            .AsNoTracking()
            .Where(x => (!ignorarId.HasValue || x.Id != ignorarId.Value) &&
                        ((cpf != null && x.Cpf == cpf) || (email != null && x.Email == email)))
            .Select(x => new
            {
                CpfDuplicado = cpf != null && x.Cpf == cpf,
                EmailDuplicado = email != null && x.Email == email
            })
            .ToListAsync();

        return (duplicidades.Any(x => x.CpfDuplicado), duplicidades.Any(x => x.EmailDuplicado));
    }
}
