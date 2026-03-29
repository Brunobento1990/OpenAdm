using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class NotificarParceiroService : INotificarParceiroService
{
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IHttpClientWhatsApp _httpClientWhatsApp;

    public NotificarParceiroService(IParceiroAutenticado parceiroAutenticado, IHttpClientWhatsApp httpClientWhatsApp)
    {
        _parceiroAutenticado = parceiroAutenticado;
        _httpClientWhatsApp = httpClientWhatsApp;
    }

    public async Task NotificarViaWhatsAppAsync(string msg)
    {
        var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();

        if (parceiro.Telefones == null || parceiro.Telefones.Count == 0)
        {
            return;
        }

        foreach (var parceiroTelefone in parceiro.Telefones)
        {
            await _httpClientWhatsApp.EnviarMsgAsync(new()
            {
                Number =  $"55{parceiroTelefone.Telefone}",
                Text = msg
            });
        }
    }
}