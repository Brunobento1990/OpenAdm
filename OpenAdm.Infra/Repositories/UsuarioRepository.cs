using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Extensions.IQueryable;

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

    public async Task<PaginacaoViewModel<Usuario>> GetPaginacaoAsync(FilterModel<Usuario> filterModel)
    {
        var (total, values) = await _parceiroContext
                .Usuarios
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Usuario>(x, filterModel.OrderBy))
                .WhereIsNotNull(filterModel.GetWhereBySearch())
                .CustomFilterAsync(filterModel);

        return new()
        {
            TotalPage = total,
            Values = values
        };
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
