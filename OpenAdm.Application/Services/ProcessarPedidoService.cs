using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Emails;

namespace OpenAdm.Application.Services;

public class ProcessarPedidoService : IProcessarPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;
    private readonly IEmailApiService _emailService;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;

    public ProcessarPedidoService(
        IPedidoRepository pedidoRepository,
        IPdfPedidoService pdfPedidoService,
        IEmailApiService emailService,
        IParceiroAutenticado parceiroAutenticado,
        IConfiguracoesDePedidoService configuracoesDePedidoService)
    {
        _pedidoRepository = pedidoRepository;
        _pdfPedidoService = pdfPedidoService;
        _emailService = emailService;
        _parceiroAutenticado = parceiroAutenticado;
        _configuracoesDePedidoService = configuracoesDePedidoService;
    }

    public async Task ProcessarCreateAsync(Guid pedidoId)
    {
        var configuracoesDePedido = await _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();
        var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId);

        if (pedido != null)
        {
            var htmlEnvio = await File.ReadAllTextAsync(Path.Combine("Htmls", "NovoPedido.html"));
            htmlEnvio = htmlEnvio.Replace("***pedido***", pedido.Numero.ToString());
            htmlEnvio = htmlEnvio.Replace("***empresa***", parceiro.NomeFantasia);
            var pdf = _pdfPedidoService.GeneratePdfPedido(pedido, parceiro);
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
}
