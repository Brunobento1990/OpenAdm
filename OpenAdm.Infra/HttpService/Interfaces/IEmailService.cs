using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmail(EnvioEmailModel envioEmailModel);
}
