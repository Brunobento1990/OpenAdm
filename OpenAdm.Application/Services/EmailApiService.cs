using OpenAdm.Application.Interfaces;
using System.Net.Mail;
using System.Net;
using OpenAdm.Application.Models.Emails;

namespace OpenAdm.Application.Services;

public sealed class EmailApiService : IEmailApiService
{
    public async Task<bool> SendEmailAsync(ToEnvioEmailApiModel envioEmailModel, FromEnvioEmailApiModel fromEnvioEmailModel)
    {
        try
        {
            var mail = new MailMessage(fromEnvioEmailModel.Email, envioEmailModel.Email)
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

            var smtp = new SmtpClient(fromEnvioEmailModel.Servidor, fromEnvioEmailModel.Porta)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEnvioEmailModel.Email, fromEnvioEmailModel.Senha)
            };
            await smtp.SendMailAsync(mail);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
