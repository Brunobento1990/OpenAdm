using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.Cached.Cached;

public class UsuarioCached : IUsuarioRepository
{
    private readonly ICachedService<Usuario> _cachedService;
    private readonly UsuarioRepository _usuarioRepository;
    private const string _keyList = "usuarios";

    public UsuarioCached(ICachedService<Usuario> cachedService, UsuarioRepository usuarioRepository)
    {
        _cachedService = cachedService;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario> AddAsync(Usuario entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _usuarioRepository.AddAsync(entity);
    }

    public async Task<bool> DeleteAsync(Usuario entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        return await _usuarioRepository.DeleteAsync(entity);
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(Guid id)
    {
        var key = id.ToString();
        var usuario = await _cachedService.GetItemAsync(key);

        if (usuario == null)
        {
            usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario != null)
            {
                await _cachedService.SetItemAsync(key, usuario);
            }
        }

        return usuario;
    }

    public async Task<Usuario> UpdateAsync(Usuario entity)
    {
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(entity.Id.ToString());
        return await _usuarioRepository.UpdateAsync(entity);
    }

    public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
    {
        return await _usuarioRepository.GetUsuarioByEmailAsync(email);
    }
}
