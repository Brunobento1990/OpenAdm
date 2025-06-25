using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Application.Models.Emails;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Application.Models;

namespace OpenAdm.Application.Services;

public class EsqueceuSenhaService : IEsqueceuSenhaService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmailApiService _emailService;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    public EsqueceuSenhaService(
        IUsuarioRepository usuarioRepository,
        IEmailApiService emailService,
        IParceiroAutenticado parceiroAutenticado)
    {
        _usuarioRepository = usuarioRepository;
        _emailService = emailService;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<bool> RecuperarSenhaAsync(EsqueceuSenhaDto esqueceuSenhaDto)
    {
        var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();
        var usuario = await _usuarioRepository.GetUsuarioByEmailAsync(esqueceuSenhaDto.Email)
            ?? throw new ExceptionApi("Não foi possível localizar seu cadastro!");

        usuario.EsqueceuSenha();

        var htmlEnvio = await File.ReadAllTextAsync(Path.Combine("Htmls", "EsqueceuSenha.html"));
        htmlEnvio = htmlEnvio.Replace("***empresa***", parceiro.NomeFantasia);
        htmlEnvio = htmlEnvio.Replace("***ecommerce***", parceiro.NomeFantasia);
        htmlEnvio = htmlEnvio.Replace("***usuario***", usuario.Nome);
        htmlEnvio = htmlEnvio.Replace("***link***", $"{parceiro.EmpresaOpenAdm.UrlEcommerce}/recuperar-senha/{usuario.TokenEsqueceuSenha}");

        var fromEnvioEmail = new FromEnvioEmailApiModel()
        {
            Email = EmailConfiguracaoModel.Email,
            EnableSsl = true,
            Porta = EmailConfiguracaoModel.Porta,
            Senha = EmailConfiguracaoModel.Senha,
            Servidor = EmailConfiguracaoModel.Servidor
        };

        var emailModel = new ToEnvioEmailApiModel()
        {
            Assunto = "Recuperar senha",
            Email = esqueceuSenhaDto.Email,
            Mensagem = "",
            Html = htmlEnvio
        };

        var result = await _emailService.SendEmailAsync(emailModel, fromEnvioEmail);

        if (result)
        {
            await _usuarioRepository.UpdateAsync(usuario);
        }
        else
        {
            throw new ExceptionApi("Não foi possível enviar o e-mail de recuperação de senha, tente novamente!");
        }

        return result;
    }
}
