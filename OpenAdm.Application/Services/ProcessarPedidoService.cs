using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using System.Text;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Emails;

namespace OpenAdm.Application.Services;

public class ProcessarPedidoService : IProcessarPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;
    private readonly IEmailApiService _emailService;


    public ProcessarPedidoService(
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IPedidoRepository pedidoRepository,
        IPdfPedidoService pdfPedidoService,
        IEmailApiService emailService)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _pedidoRepository = pedidoRepository;
        _pdfPedidoService = pdfPedidoService;
        _emailService = emailService;
    }

    public async Task ProcessarCreateAsync(Guid pedidoId)
    {
        var configuracoesDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync()
            ?? throw new Exception("Configurações de pedido inválida!");

        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId);

        if (pedido != null)
        {
            var logo = configuracoesDePedido.Logo != null ? Encoding.UTF8.GetString(configuracoesDePedido.Logo) : null;
            var pdf = _pdfPedidoService.GeneratePdfPedido(pedido, "teste", logo);

            var message = $"Que ótima noticia, mais um pedido!\nN. do pedido : {pedido.Numero}";
            var assunto = "Novo pedido";

            var emailModel = new ToEnvioEmailApiModel()
            {
                Assunto = assunto,
                Email = configuracoesDePedido.EmailDeEnvio,
                Mensagem = message,
                Arquivo = pdf,
                NomeDoArquivo = $"pedido-{pedido.Numero}",
                TipoDoArquivo = "application/pdf"
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
