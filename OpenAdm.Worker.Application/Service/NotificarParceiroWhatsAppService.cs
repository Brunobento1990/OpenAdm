using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Eventos;
using OpenAdm.Worker.Application.DTOs;
using OpenAdm.Worker.Application.HttpService.Interface;
using OpenAdm.Worker.Application.HttpService.Request;
using OpenAdm.Worker.Application.Interfaces;

namespace OpenAdm.Worker.Application.Service;

public class NotificarParceiroWhatsAppService : IEventoAplicacaoService
{
    private readonly IParceiroRepository _parceiroRepository;
    private readonly IHttpClientWhatsApp _httpClientWhatsApp;

    public NotificarParceiroWhatsAppService(IParceiroRepository parceiroRepository,
        IHttpClientWhatsApp httpClientWhatsApp)
    {
        _parceiroRepository = parceiroRepository;
        _httpClientWhatsApp = httpClientWhatsApp;
    }

    public async Task<ResultPartner<ResultadoEventoAplicacaoDTO>> ExecutarAsync(EventoAplicacao eventoAplicacao)
    {
        var notificarParceiroWhatsAppEvento = eventoAplicacao.DadosParse<NotificarParceiroWhatsAppEvento>();

        if (notificarParceiroWhatsAppEvento == null)
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)"Dados null para notificar parceiro";
        }

        var parceiro = await _parceiroRepository.ObterPorEmpresaOpenAdmIdAsync(eventoAplicacao.EmpresaOpenAdmId);

        if (parceiro == null)
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)
                "Não foi possível localizar o parceiro para envio da notificação";
        }

        foreach (var telefone in parceiro.Telefones)
        {
            await _httpClientWhatsApp.EnviarMsgAsync(new EnviarMsgWppRequest()
            {
                Number = $"55{telefone.Telefone.LimparMascaraTelefone()}",
                Text = notificarParceiroWhatsAppEvento.Mensagem
            });
        }

        return (ResultPartner<ResultadoEventoAplicacaoDTO>)new ResultadoEventoAplicacaoDTO()
        {
            Mensagem = $"Notificação do parceiro: {parceiro.NomeFantasia} =>  efetuada com sucesso"
        };
    }
}