using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class TopUsuariosRepository : ITopUsuariosRepository
{
    private readonly AppDbContext _appDbContext;

    public TopUsuariosRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(TopUsuario topUsuario)
    {
        await _appDbContext.AddAsync(topUsuario);
    }

    public async Task<TopUsuario?> GetByUsuarioIdAsync(Guid usuarioId, Guid parceiroId)
    {
        return await _appDbContext
            .TopUsuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.ParceiroId == parceiroId);
    }

    public async Task<IList<TopUsuario>> GetTopTresUsuariosToTalCompraAsync(Guid parceiroId)
    {
        return await _appDbContext
            .TopUsuarios
            .AsNoTracking()
            .Where(x => x.ParceiroId == parceiroId)
            .OrderByDescending(x => x.TotalCompra)
            .Skip(0)
            .Take(3)
            .ToListAsync();
    }

    public async Task<IList<TopUsuario>> GetTopTresUsuariosToTalPedidosAsync(Guid parceiroId)
    {
        return await _appDbContext
            .TopUsuarios
            .AsNoTracking()
            .Where(x => x.ParceiroId == parceiroId)
            .OrderByDescending(x => x.TotalPedidos)
            .Skip(0)
            .Take(3)
            .ToListAsync();
    }

    public async Task UpdateAsync(TopUsuario topUsuario)
    {
        _appDbContext.Attach(topUsuario);
        _appDbContext.Update(topUsuario);
        await _appDbContext.SaveChangesAsync();
    }
}
