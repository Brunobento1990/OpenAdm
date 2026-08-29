using Microsoft.Extensions.Configuration;
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

public class NotificarNovoPedidoService : IEventoAplicacaoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IParceiroRepository _parceiroRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IHttpClientWhatsApp _httpClientWhatsApp;
    private readonly IEmailService _emailService;
    private readonly string _openAdmApiBaseUrl;

    public NotificarNovoPedidoService(IPedidoRepository pedidoRepository,
        IParceiroRepository parceiroRepository,
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IHttpClientWhatsApp httpClientWhatsApp,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _pedidoRepository = pedidoRepository;
        _parceiroRepository = parceiroRepository;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _httpClientWhatsApp = httpClientWhatsApp;
        _emailService = emailService;
        _openAdmApiBaseUrl = configuration["OpenAdmApi:BaseUrl"]?.TrimEnd('/') ?? string.Empty;
    }

    public async Task<ResultPartner<ResultadoEventoAplicacaoDTO>> ExecutarAsync(EventoAplicacao eventoAplicacao)
    {
        var dadosNovoPedido = eventoAplicacao.DadosParse<NovoPedidoEvento>();

        if (dadosNovoPedido == null)
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)"Os dados do pedido são NULL";
        }

        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(dadosNovoPedido.PedidoId);

        if (pedido == null)
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)
                "Não foi possível localizar o pedido para envio da notificação";
        }

        var parceiro = await _parceiroRepository.ObterPorEmpresaOpenAdmIdAsync(eventoAplicacao.EmpresaOpenAdmId);

        if (parceiro == null)
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)
                "Não foi possível localizar o parceiro para envio da notificação";
        }

        var configuracoesDePedido =
            await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(eventoAplicacao.EmpresaOpenAdmId);

        if (configuracoesDePedido == null)
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)
                "Não foi possível localizar a configuração de pedido para envio da notificação";
        }

        if (string.IsNullOrWhiteSpace(_openAdmApiBaseUrl))
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)
                "Não foi possível localizar a URL pública da API para envio da notificação";
        }

        var linkPedido = $"{_openAdmApiBaseUrl}/publico/{eventoAplicacao.EmpresaOpenAdmId}/pedido/{pedido.IdPublico}/pdf";

        if (!string.IsNullOrWhiteSpace(configuracoesDePedido.WhatsApp))
        {
            var payload = new EnviarMsgWppRequest()
            {
                Number = $"55{configuracoesDePedido.WhatsApp.LimparMascaraTelefone()}",
                Text =
                    $"🛒 Novo pedido confirmado!\nParceiro: {parceiro.NomeFantasia}\nCliente: {pedido.Usuario.Nome}\nPedido: #{pedido.Numero}\nTotal: {pedido.ValorTotal.FormatMoney()}\n\nPDF do pedido:\n{linkPedido}"
            };

            var resultado = await _httpClientWhatsApp.EnviarMsgAsync(payload);

            if (resultado)
            {
                return new ResultPartner<ResultadoEventoAplicacaoDTO>()
                {
                    Result = new ResultadoEventoAplicacaoDTO()
                    {
                        Mensagem = "Notificação de novo pedido enviado com sucesso via whats app!"
                    }
                };
            }
        }

        var htmlEnvio = await File.ReadAllTextAsync(Path.Combine("Htmls", "NovoPedido.html"));
        htmlEnvio = htmlEnvio.Replace("***pedido***", pedido.Numero.ToString());
        htmlEnvio = htmlEnvio.Replace("***empresa***", parceiro.NomeFantasia);
        htmlEnvio = htmlEnvio.Replace("***linkPedido***", linkPedido);

        var emailModel = new EnviarEmailDTO()
        {
            Assunto = "Novo pedido",
            Email = configuracoesDePedido.EmailDeEnvio,
            Html = htmlEnvio
        };

        await _emailService.EnviarAsync(emailModel);

        return new ResultPartner<ResultadoEventoAplicacaoDTO>()
        {
            Result = new ResultadoEventoAplicacaoDTO()
            {
                Mensagem = "Notificação de novo pedido enviado com sucesso!"
            }
        };
    }
}
