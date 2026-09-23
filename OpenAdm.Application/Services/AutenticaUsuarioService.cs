using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class AutenticaUsuarioService : IAutenticaUsuarioService
{
    private readonly ISessaoUsuarioRepository _sessaoUsuarioRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public AutenticaUsuarioService(
        ISessaoUsuarioRepository sessaoUsuarioRepository,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _sessaoUsuarioRepository = sessaoUsuarioRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public Task<ResultPartner<bool>> ValidarAsync()
        => _usuarioAutenticado.IsFuncionario
            ? ValidaFuncionarioAsync()
            : ValidaUsuarioAsync();

    private async Task<ResultPartner<bool>> ValidaFuncionarioAsync()
    {
        var funcionario = await _usuarioAutenticado.GetFuncionarioMiddlewareAsync();

        if (funcionario == null)
        {
            await _sessaoUsuarioRepository.DerrubarSessaoAsync(_usuarioAutenticado.SessaoId);
            return (ResultPartner<bool>)"Seu cadastro não foi localizado!";
        }

        if (!funcionario.Ativo)
        {
            await _sessaoUsuarioRepository.DerrubarSessaoAsync(_usuarioAutenticado.SessaoId);
            return (ResultPartner<bool>)"Seu acesso esta bloqueado!";
        }

        return (ResultPartner<bool>)true;
    }

    private async Task<ResultPartner<bool>> ValidaUsuarioAsync()
    {
        var usuario = await _usuarioAutenticado.GetUsuarioMiddlewareAsync();

        if (usuario == null)
        {
            await _sessaoUsuarioRepository.DerrubarSessaoAsync(_usuarioAutenticado.SessaoId);
            return (ResultPartner<bool>)"Seu acesso esta bloqueado!";
        }

        if (!usuario.AcessoLiberadoEcommerce)
        {
            await _sessaoUsuarioRepository.DerrubarSessaoAsync(_usuarioAutenticado.SessaoId);
            return (ResultPartner<bool>)"Seu acesso esta bloqueado!";
        }

        return (ResultPartner<bool>)true;
    }
}