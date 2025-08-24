using OpenAdm.Application.Dtos.WhatsApp;

namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IChatWppHttpClient
{
    Task<bool> EnviarPdfAsync(EnviarPDFWppDTO enviarPDFWppDTO);
}
