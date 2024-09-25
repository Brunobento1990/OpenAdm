using OpenAdm.Application.Models.Emails;

namespace OpenAdm.Application.Interfaces;

public interface IEmailApiService
{
    Task<bool> SendEmailAsync(ToEnvioEmailApiModel envioEmailModel, FromEnvioEmailApiModel fromEnvioEmailModel);
}
