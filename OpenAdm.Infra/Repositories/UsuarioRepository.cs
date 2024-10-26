using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.Repositories;

public class UsuarioRepository(ParceiroContext parceiroContext)
    : GenericRepository<Usuario>(parceiroContext), IUsuarioRepository
{
    public async Task<IList<Usuario>> GetAllUsuariosAsync()
    {
        return await _parceiroContext
            .Usuarios
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
    {
        return await _parceiroContext
             .Usuarios
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
