using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Emails;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Interfaces;
using static BCrypt.Net.BCrypt;

namespace OpenAdm.Application.Services;

public class EsqueceuSenhaService : IEsqueceuSenhaService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmailService _emailService;

    public EsqueceuSenhaService(
        IUsuarioRepository usuarioRepository,
        IEmailService emailService)
    {
        _usuarioRepository = usuarioRepository;
        _emailService = emailService;
    }

    public async Task<bool> EsqueceuSenhaAsync(EsqueceuSenhaDto esqueceuSenhaDto)
    {
        var usuario = await _usuarioRepository.GetUsuarioByEmailAsync(esqueceuSenhaDto.Email)
            ?? throw new Exception(CodigoErrors.ErrorGeneric);

        var senha = GenerateSenha.Generate();


        var message = $"Recuperação de senha efetuada com sucesso!\nSua nova senha é {senha} .\nImportante!\nNo Próximo acesso ao nosso site, efetue a troca da senha.\nCaso não tenha feito o pedido de recuperação de senha, por favor, entre em contato com o suporte!.";
        var assunto = "Recuperação de senha";

        var emailModel = new EnvioEmailModel()
        {
            Assunto = assunto,
            Email = esqueceuSenhaDto.Email,
            Mensagem = message
        };

        var result = await _emailService.SendEmail(emailModel);

        if (result)
        {
            var newSenha = HashPassword(senha, 10);
            usuario.UpdateSenha(newSenha);
            await _usuarioRepository.UpdateAsync(usuario);
        }

        return result;
    }
}
