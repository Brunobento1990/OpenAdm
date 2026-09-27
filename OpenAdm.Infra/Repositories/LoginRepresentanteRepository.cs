using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public sealed class LoginRepresentanteRepository(ParceiroContext parceiroContext)
    : ILoginRepresentanteRepository
{
    public Task<Representante?> ObterPorEmailAsync(string email) =>
        parceiroContext.Representantes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
}
