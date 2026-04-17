using OpenAdm.Domain.Model;
using OpenAdm.Worker.Application.HttpService.Request;
using OpenAdm.Worker.Application.HttpService.Response;

namespace OpenAdm.Worker.Application.HttpService.Interface;

public interface IHttpClientWhatsApp
{
    Task<bool> EnviarPdfAsync(EnviarPDFWppRequest request);
    Task<bool> EnviarMsgAsync(EnviarMsgWppRequest request);
    Task<ResultPartner<StatusConexaoWhatsAppResponse>> StatusConexaoAsync();
}