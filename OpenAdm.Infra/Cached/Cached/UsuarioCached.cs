using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Cached.Interfaces;
using OpenAdm.Infra.Repositories;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

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
        var key = $"usuario-{entity.Id}";
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(key);
        return await _usuarioRepository.DeleteAsync(entity);
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(Guid id)
    {
        var key = $"usuario-{id}";
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
        var key = $"usuario-{entity.Id}";
        await _cachedService.RemoveCachedAsync(_keyList);
        await _cachedService.RemoveCachedAsync(key);
        return await _usuarioRepository.UpdateAsync(entity);
    }

    public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
    {
        return await _usuarioRepository.GetUsuarioByEmailAsync(email);
    }

    public async Task<IList<Usuario>> GetAllUsuariosAsync()
    {
        var usuarios = await _cachedService.GetListItemAsync(_keyList);

        if (usuarios is null)
        {
            usuarios = await _usuarioRepository.GetAllUsuariosAsync();

            if (usuarios?.Count > 0)
            {
                await _cachedService.SetListItemAsync(_keyList, usuarios);
            }
        }

        return usuarios ?? new List<Usuario>();
    }

    public Task<PaginacaoViewModel<Usuario>> GetPaginacaoAsync(FilterModel<Usuario> filterModel)
        => _usuarioRepository.PaginacaoAsync(filterModel);

    public Task<PaginacaoViewModel<Usuario>> PaginacaoAsync(FilterModel<Usuario> filterModel)
        => _usuarioRepository.PaginacaoAsync(filterModel);

    public Task<IList<Usuario>> PaginacaoDropDownAsync(PaginacaoDropDown<Usuario> paginacaoDropDown)
        => _usuarioRepository.PaginacaoDropDownAsync(paginacaoDropDown);

    public Task<Usuario?> GetUsuarioByCpfAsync(string cpf)
        => _usuarioRepository.GetUsuarioByCpfAsync(cpf);

    public Task<Usuario?> GetUsuarioByCnpjAsync(string cnpj)
        => _usuarioRepository.GetUsuarioByCnpjAsync(cnpj);

    public Task<int> GetCountAsync()
        => _usuarioRepository.GetCountAsync();

    public Task<int> SaveChangesAsync()
        => _usuarioRepository.SaveChangesAsync();
}
