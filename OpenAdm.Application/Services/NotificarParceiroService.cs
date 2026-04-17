using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class NotificarParceiroService : INotificarParceiroService
{
    private readonly IParceiroAutenticado _parceiroAutenticado;

    public NotificarParceiroService(IParceiroAutenticado parceiroAutenticado)
    {
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task NotificarViaWhatsAppAsync(string msg)
    {
        //TODO:
        // var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();
        //
        // if (parceiro.Telefones == null || parceiro.Telefones.Count == 0)
        // {
        //     return;
        // }
        //
        // foreach (var parceiroTelefone in parceiro.Telefones)
        // {
        //     await _httpClientWhatsApp.EnviarMsgAsync(new()
        //     {
        //         Number =  $"55{parceiroTelefone.Telefone}",
        //         Text = msg
        //     });
        // }
    }
}