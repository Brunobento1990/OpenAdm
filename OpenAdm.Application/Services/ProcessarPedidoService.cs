using OpenAdm.Application.Dtos.WhatsApp;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.ConfiguracoesDePedidos;
using OpenAdm.Application.Models.Emails;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ProcessarPedidoService : IProcessarPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;
    private readonly IEmailApiService _emailService;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IChatWppHttpClient _chatWppHttpClient;
    private readonly IEstoqueService _estoqueService;

    public ProcessarPedidoService(
        IPedidoRepository pedidoRepository,
        IPdfPedidoService pdfPedidoService,
        IEmailApiService emailService,
        IParceiroAutenticado parceiroAutenticado,
        IChatWppHttpClient chatWppHttpClient, IEstoqueService estoqueService)
    {
        _pedidoRepository = pedidoRepository;
        _pdfPedidoService = pdfPedidoService;
        _emailService = emailService;
        _parceiroAutenticado = parceiroAutenticado;
        _chatWppHttpClient = chatWppHttpClient;
        _estoqueService = estoqueService;
    }

    public async Task ProcessarCreateAsync(Guid pedidoId, ConfiguracoesDePedidoViewModel configuracoesDePedido)
    {
        var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId);

        if (pedido == null)
        {
            return;
        }
        
        await _estoqueService.ReservarEstoqueNovoPedidoAsync(pedido);

        var pdf = _pdfPedidoService.GeneratePdfPedido(pedido, parceiro);

        if (!string.IsNullOrWhiteSpace(configuracoesDePedido.WhatsApp))
        {
            var payload = new EnviarPDFWppDTO()
            {
                Number = $"55{configuracoesDePedido.WhatsApp.LimparMascaraTelefone()}",
                MediaType = "document",
                MimeType = "application/pdf",
                Caption =
                    $"{parceiro.NomeFantasia}\nÓtima noticia, o cliente {pedido.Usuario.Nome} fez um novo pedido \nNúmero do pedido: {pedido.Numero}",
                Media = Convert.ToBase64String(pdf),
                FileName = $"pedido-{pedido.Numero}.pdf",
                Delay = 0,
                LinkPreview = false,
                MentionsEveryOne = false,
                Mentioned = null!
            };

            var resultado = await _chatWppHttpClient.EnviarPdfAsync(payload);

            if (resultado)
            {
                return;
            }
        }

        var htmlEnvio = await File.ReadAllTextAsync(Path.Combine("Htmls", "NovoPedido.html"));
        htmlEnvio = htmlEnvio.Replace("***pedido***", pedido.Numero.ToString());
        htmlEnvio = htmlEnvio.Replace("***empresa***", parceiro.NomeFantasia);

        var assunto = "Novo pedido";

        var emailModel = new ToEnvioEmailApiModel()
        {
            Assunto = assunto,
            Email = configuracoesDePedido.EmailDeEnvio,
            Arquivo = pdf,
            NomeDoArquivo = $"pedido-{pedido.Numero}",
            TipoDoArquivo = "application/pdf",
            Html = htmlEnvio
        };

        await _emailService.SendEmailAsync(emailModel, new FromEnvioEmailApiModel()
        {
            Email = EmailConfiguracaoModel.Email,
            Porta = EmailConfiguracaoModel.Porta,
            Senha = EmailConfiguracaoModel.Senha,
            Servidor = EmailConfiguracaoModel.Servidor
        });
    }
}