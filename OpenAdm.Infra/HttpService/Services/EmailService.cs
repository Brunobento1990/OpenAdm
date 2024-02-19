using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.Model;
using System.Net.Mail;
using System.Net;

namespace OpenAdm.Infra.HttpService.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguracaoDeEmailRepository _configuracaoDeEmailRepository;

    public EmailService(IConfiguracaoDeEmailRepository configuracaoDeEmailRepository)
    {
        _configuracaoDeEmailRepository = configuracaoDeEmailRepository;
    }

    public async Task<bool> SendEmail(EnvioEmailModel envioEmailModel)
    {
        var configuracao = await _configuracaoDeEmailRepository.GetConfiguracaoDeEmailAtivaAsync()
            ?? throw new Exception("Configuração de eamil não encontrada!");

        try
        {
            var mail = new MailMessage(configuracao.Email, envioEmailModel.Email)
            {
                Subject = envioEmailModel.Assunto,
                SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8"),
                BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8"),
                Body = envioEmailModel.Mensagem
            };

            if (envioEmailModel.Arquivo != null && !string.IsNullOrWhiteSpace(envioEmailModel.NomeDoArquivo) && !string.IsNullOrWhiteSpace(envioEmailModel.TipoDoArquivo))
            {
                var anexo = new Attachment(new MemoryStream(envioEmailModel.Arquivo), envioEmailModel.NomeDoArquivo, envioEmailModel.TipoDoArquivo);
                mail.Attachments.Add(anexo);
            }

            var smtp = new SmtpClient(configuracao.Servidor, configuracao.Porta);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(configuracao.Email, configuracao.Senha);
            smtp.Send(mail);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
