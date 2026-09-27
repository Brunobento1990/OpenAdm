using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Enuns;

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
        => _usuarioAutenticado.TipoUsuario switch
        {
            TipoUsuario.Funcionario => ValidaFuncionarioAsync(),
            TipoUsuario.Representante => ValidaRepresentanteAsync(),
            TipoUsuario.Usuario => ValidaUsuarioAsync(),
            _ => Task.FromResult((ResultPartner<bool>)"Tipo de usuário inválido")
        };

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

    private async Task<ResultPartner<bool>> ValidaRepresentanteAsync()
    {
        var representante = await _usuarioAutenticado.GetRepresentanteMiddlewareAsync();

        if (representante is { Ativo: true })
            return (ResultPartner<bool>)true;

        await _sessaoUsuarioRepository.DerrubarSessaoAsync(_usuarioAutenticado.SessaoId);
        return (ResultPartner<bool>)"Seu acesso esta bloqueado!";
    }
}
