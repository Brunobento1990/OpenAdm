using Domain.Pkg.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;

namespace OpenAdm.Infra.Cached.Cached;

public sealed class TopUsuariosCached : ITopUsuariosRepository
{
    private readonly TopUsuariosRepository _topUsuariosRepository;
    private readonly ICachedService<TopUsuarios> _cachedService;
    private const string _totalPedidos = "TotalPedidos";
    private const string _totalCompra = "TotalCompra";

    public TopUsuariosCached(TopUsuariosRepository topUsuariosRepository, ICachedService<TopUsuarios> cachedService)
    {
        _topUsuariosRepository = topUsuariosRepository;
        _cachedService = cachedService;
    }

    public async Task<IList<TopUsuarios>> GetTopTresUsuariosToTalCompraAsync()
    {
        var topUsuarios = await _cachedService.GetListItemAsync(_totalCompra);

        if (topUsuarios == null)
        {
            topUsuarios = await _topUsuariosRepository.GetTopTresUsuariosToTalCompraAsync();

            if (topUsuarios.Count > 0)
            {
                await _cachedService.SetListItemAsync(_totalCompra, topUsuarios);
            }
        }

        return topUsuarios;
    }

    public async Task<IList<TopUsuarios>> GetTopTresUsuariosToTalPedidosAsync()
    {
        var topUsuarios = await _cachedService.GetListItemAsync(_totalPedidos);

        if (topUsuarios == null)
        {
            topUsuarios = await _topUsuariosRepository.GetTopTresUsuariosToTalPedidosAsync();

            if (topUsuarios.Count > 0)
            {
                await _cachedService.SetListItemAsync(_totalPedidos, topUsuarios);
            }
        }

        return topUsuarios;
    }

    public Task AddAsync(TopUsuarios topUsuario)
        => _topUsuariosRepository.AddAsync(topUsuario);

    public Task UpdateAsync(TopUsuarios topUsuario)
        => _topUsuariosRepository.UpdateAsync(topUsuario);

    public Task<TopUsuarios?> GetByUsuarioIdAsync(Guid usuarioId)
        => _topUsuariosRepository.GetByUsuarioIdAsync(usuarioId);
}
