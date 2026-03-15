using OpenAdm.Application.Dtos.WhatsApp;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IHttpClientWhatsApp
{
    Task<bool> EnviarPdfAsync(EnviarPDFWppDTO enviarPDFWppDTO);
    Task<ResultPartner<StatusConexaoWhatsAppResponse>> StatusConexaoAsync();
}