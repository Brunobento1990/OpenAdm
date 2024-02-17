using OpenAdm.Application.Models.Emails;

namespace OpenAdm.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmail(EnvioEmailModel envioEmailModel);
}
