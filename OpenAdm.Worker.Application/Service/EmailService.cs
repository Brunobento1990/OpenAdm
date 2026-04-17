using Microsoft.Extensions.Configuration;
using OpenAdm.Worker.Application.DTOs;
using OpenAdm.Worker.Application.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace OpenAdm.Worker.Application.Service;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> EnviarAsync(EnviarEmailDTO enviarEmailDTO)
    {
        try
        {
            var mail = new MailMessage(_configuration["Email:Email"]!, enviarEmailDTO.Email)
            {
                Subject = enviarEmailDTO.Assunto,
                SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8"),
                BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8"),
                Body = enviarEmailDTO.Mensagem
            };

            if (enviarEmailDTO.Arquivo != null && !string.IsNullOrWhiteSpace(enviarEmailDTO.NomeDoArquivo) &&
                !string.IsNullOrWhiteSpace(enviarEmailDTO.TipoDoArquivo))
            {
                var anexo = new Attachment(new MemoryStream(enviarEmailDTO.Arquivo), enviarEmailDTO.NomeDoArquivo,
                    enviarEmailDTO.TipoDoArquivo);
                mail.Attachments.Add(anexo);
            }

            if (!string.IsNullOrWhiteSpace(enviarEmailDTO.Html))
            {
                mail.AlternateViews.Add(
                    AlternateView.CreateAlternateViewFromString(enviarEmailDTO.Html, null, MediaTypeNames.Text.Html));
            }

            var smtp = new SmtpClient(_configuration["Email:Servidor"]!, int.Parse(_configuration["Email:Porta"]!))
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration["Email:Email"]!, _configuration["Email:Senha"]!)
            };
            await smtp.SendMailAsync(mail);

            return true;
        }
        catch (Exception e)
        {
            LogService.Error(e.InnerException?.Message ?? e.Message);
            return false;
        }
    }
}