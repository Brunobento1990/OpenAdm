using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.Repositories;

public class LoginUsuarioRepository(ParceiroContext parceiroContext) 
    : ILoginUsuarioRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<Usuario?> LoginAsync(string email)
    {
        return await _parceiroContext
            .Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}
