using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class TopUsuariosRepository : ITopUsuariosRepository
{
    private readonly ParceiroContext _parceiroContext;

    public TopUsuariosRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task AddAsync(TopUsuarios topUsuario)
    {
        await _parceiroContext.AddAsync(topUsuario);
        await _parceiroContext.SaveChangesAsync();
    }

    public async Task<TopUsuarios?> GetByUsuarioIdAsync(Guid usuarioId)
    {
        return await _parceiroContext
            .TopUsuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
    }

    public async Task<IList<TopUsuarios>> GetTopTresUsuariosToTalCompraAsync()
    {
        return await _parceiroContext
            .TopUsuarios
            .AsNoTracking()
            .OrderByDescending(x => x.TotalCompra)
            .Skip(0)
            .Take(3)
            .ToListAsync();
    }

    public async Task<IList<TopUsuarios>> GetTopTresUsuariosToTalPedidosAsync()
    {
        return await _parceiroContext
            .TopUsuarios
            .AsNoTracking()
            .OrderByDescending(x => x.TotalPedidos)
            .Skip(0)
            .Take(3)
            .ToListAsync();
    }

    public async Task UpdateAsync(TopUsuarios topUsuario)
    {
        _parceiroContext.Attach(topUsuario);
        _parceiroContext.Update(topUsuario);
        await _parceiroContext.SaveChangesAsync();
    }
}
