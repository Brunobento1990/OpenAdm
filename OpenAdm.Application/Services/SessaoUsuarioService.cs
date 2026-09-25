using Microsoft.Extensions.Configuration;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Services;

public class SessaoUsuarioService(
    ISessaoUsuarioRepository sessaoUsuarioRepository,
    IParceiroAutenticado parceiroAutenticado,
    IUsuarioSessaoRequest usuarioSessaoRequest,
    IConfiguration configuration) : ISessaoUsuarioService
{
    public async Task<SessaoUsuario> CriarAsync(Guid usuarioId, TipoUsuario tipoUsuario)
    {
        var dias = int.TryParse(configuration["SessaoUsuario:ExpiracaoDias"], out var diasConfigurados)
            ? diasConfigurados
            : 10;

        var sessao = SessaoUsuario.Criar(
            usuarioId,
            parceiroAutenticado.Id,
            tipoUsuario,
            dias,
            usuarioSessaoRequest);

        await sessaoUsuarioRepository.AdicionarAsync(sessao);
        await sessaoUsuarioRepository.SalvarAlteracoesAsync();

        return sessao;
    }

    public Task DerrubarSessaoAsync(Guid sessaoId)
        => sessaoUsuarioRepository.DerrubarSessaoAsync(sessaoId);

    public Task DerrubarSessoesAsync(Guid usuarioId, TipoUsuario tipoUsuario)
        => sessaoUsuarioRepository.DerrubarSessoesAsync(usuarioId, parceiroAutenticado.Id, tipoUsuario);
}
