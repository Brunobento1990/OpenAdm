using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using Domain.Pkg.Errors;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Application.Adapters;
using OpenAdm.Domain.Helpers;
using OpenAdm.Application.Models.Emails;

namespace OpenAdm.Application.Services;

public class EsqueceuSenhaService : IEsqueceuSenhaService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmailApiService _emailService;
    private readonly IConfiguracaoDeEmailRepository _configuracaoDeEmailRepository;

    public EsqueceuSenhaService(
        IUsuarioRepository usuarioRepository,
        IEmailApiService emailService,
        IConfiguracaoDeEmailRepository configuracaoDeEmailRepository)
    {
        _usuarioRepository = usuarioRepository;
        _emailService = emailService;
        _configuracaoDeEmailRepository = configuracaoDeEmailRepository;
    }

    public async Task<bool> RecuperarSenhaAsync(EsqueceuSenhaDto esqueceuSenhaDto)
    {
        var configuracao = await _configuracaoDeEmailRepository.GetConfiguracaoDeEmailAtivaAsync()
            ?? throw new Exception("Configuração de eamil não encontrada!");

        var usuario = await _usuarioRepository.GetUsuarioByEmailAsync(esqueceuSenhaDto.Email)
            ?? throw new Exception(CodigoErrors.ErrorGeneric);

        var senha = GenerateSenha.Generate();


        var message = $"Recuperação de senha efetuada com sucesso!\nSua nova senha é {senha} .\nImportante!\nNo Próximo acesso ao nosso site, efetue a troca da senha.\nCaso não tenha feito o pedido de recuperação de senha, por favor, entre em contato com o suporte!";
        var assunto = "Recuperação de senha";

        var fromEnvioEmail = new FromEnvioEmailApiModel()
        {
            Email = configuracao.Email,
            EnableSsl = true,
            Porta = configuracao.Porta,
            Senha = Criptografia.Decrypt(configuracao.Senha),
            Servidor = configuracao.Servidor
        };

        var emailModel = new ToEnvioEmailApiModel()
        {
            Assunto = assunto,
            Email = esqueceuSenhaDto.Email,
            Mensagem = message
        };

        var result = await _emailService.SendEmailAsync(emailModel, fromEnvioEmail);

        if (result)
        {
            var newSenha = PasswordAdapter.GenerateHash(senha);
            usuario.UpdateSenha(newSenha);
            await _usuarioRepository.UpdateAsync(usuario);
        }
        else
        {
            throw new Exception(CodigoErrors.ErrorGeneric);
        }

        return result;
    }
}
