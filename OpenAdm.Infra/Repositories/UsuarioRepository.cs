using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;
using Npgsql;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Infra.Repositories;

public class UsuarioRepository(ParceiroContext parceiroContext)
    : GenericRepository<Usuario>(parceiroContext), IUsuarioRepository
{
    public override async Task<Usuario> AddAsync(Usuario entity)
    {
        try
        {
            return await base.AddAsync(entity);
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new ExceptionApi("Este e-mail, ou cpf ou cnpj já se encontra cadastrado!");
        }
    }
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
