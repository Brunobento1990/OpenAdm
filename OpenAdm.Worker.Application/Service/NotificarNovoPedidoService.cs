using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Eventos;
using OpenAdm.Pdf.Interfaces;
using OpenAdm.Worker.Application.DTOs;
using OpenAdm.Worker.Application.HttpService.Interface;
using OpenAdm.Worker.Application.HttpService.Request;
using OpenAdm.Worker.Application.Interfaces;

namespace OpenAdm.Worker.Application.Service;

public class NotificarNovoPedidoService : IEventoAplicacaoService
{
    private readonly IPdfPedidoService _pdfPedidoService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IParceiroRepository _parceiroRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IHttpClientWhatsApp _httpClientWhatsApp;
    private readonly IEmailService _emailService;

    public NotificarNovoPedidoService(IPdfPedidoService pdfPedidoService, 
        IPedidoRepository pedidoRepository, 
        IParceiroRepository parceiroRepository, 
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository, 
        IHttpClientWhatsApp httpClientWhatsApp, 
        IEmailService emailService)
    {
        _pdfPedidoService = pdfPedidoService;
        _pedidoRepository = pedidoRepository;
        _parceiroRepository = parceiroRepository;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _httpClientWhatsApp = httpClientWhatsApp;
        _emailService = emailService;
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
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)"Não foi possível localizar o pedido para envio da notificação";
        }

        var parceiro = await _parceiroRepository.ObterPorEmpresaOpenAdmIdAsync(eventoAplicacao.EmpresaOpenAdmId);

        if (parceiro == null)
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)"Não foi possível localizar o parceiro para envio da notificação";
        }

        var pdf = _pdfPedidoService.GeneratePdfPedido(pedido, parceiro);

        var configuracoesDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync(eventoAplicacao.EmpresaOpenAdmId);

        if (configuracoesDePedido == null)
        {
            return (ResultPartner<ResultadoEventoAplicacaoDTO>)"Não foi possível localizar a configuração de pedido para envio da notificação";
        }

        if (!string.IsNullOrWhiteSpace(configuracoesDePedido.WhatsApp))
        {
            var payload = new EnviarPDFWppRequest()
            {
                Number = $"55{configuracoesDePedido.WhatsApp.LimparMascaraTelefone()}",
                Mediatype = "document",
                Mimetype = "application/pdf",
                Caption =
                    $"🛒 *Novo pedido confirmado!*\n{parceiro.NomeFantasia}\n👤 Cliente: {pedido.Usuario.Nome}\n🧾 Pedido: #{pedido.Numero}\n💰 Total: {pedido.ValorTotal.FormatMoney()}.",
                Media = Convert.ToBase64String(pdf),
                FileName = $"pedido-{pedido.Numero}.pdf",
                Delay = 0,
                LinkPreview = false,
                MentionsEveryOne = false,
                Mentioned = null!
            };

            var resultado = await _httpClientWhatsApp.EnviarPdfAsync(payload);

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

        var emailModel = new EnviarEmailDTO()
        {
            Assunto = "Novo pedido",
            Email = configuracoesDePedido.EmailDeEnvio,
            Arquivo = pdf,
            NomeDoArquivo = $"pedido-{pedido.Numero}",
            TipoDoArquivo = "application/pdf",
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