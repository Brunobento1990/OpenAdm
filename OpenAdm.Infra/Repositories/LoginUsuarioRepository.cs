using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

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
