using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Dtos.Representantes;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Representantes;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public sealed class LoginRepresentanteService(
    ILoginRepresentanteRepository loginRepresentanteRepository,
    ISessaoUsuarioService sessaoUsuarioService,
    ITokenService tokenService) : ILoginRepresentanteService
{
    public async Task<ResultPartner<ResponseLoginRepresentanteViewModel>> LoginAsync(LoginRepresentanteDto dto)
    {
        var erro = dto.Validar();
        if (erro != null)
            return (ResultPartner<ResponseLoginRepresentanteViewModel>)erro;

        var email = dto.Email.Trim().ToLowerInvariant();
        var representante = await loginRepresentanteRepository.ObterPorEmailAsync(email);

        if (representante == null ||
            !representante.Ativo ||
            string.IsNullOrWhiteSpace(representante.Senha) ||
            !PasswordAdapter.VerifyPassword(dto.Senha, representante.Senha))
        {
            return (ResultPartner<ResponseLoginRepresentanteViewModel>)"E-mail ou senha inválidos!";
        }

        var sessao = await sessaoUsuarioService.CriarAsync(representante.Id, TipoUsuario.Representante);
        var token = tokenService.GenerateToken(sessao);
        var response = new ResponseLoginRepresentanteViewModel(
            token,
            RepresentanteViewModel.FromEntity(representante));

        return (ResultPartner<ResponseLoginRepresentanteViewModel>)response;
    }
}
