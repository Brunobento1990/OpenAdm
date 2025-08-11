using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;

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
            .FirstOrDefaultAsync(x => x.Cpf == email || x.Cnpj == email && x.Ativo);
    }

    public async Task<Usuario?> LoginComGoogleAsync(string email)
    {
        return await _parceiroContext
            .Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email && x.Ativo);
    }
}
