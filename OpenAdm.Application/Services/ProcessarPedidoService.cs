using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Application.Mensageria.Interfaces;
using Domain.Pkg.Entities;
using Domain.Pkg.Model;
using Domain.Pkg.Pdfs.Services;
using System.Text;
using OpenAdm.Application.Interfaces.Pedidos;
using Domain.Pkg.Interfaces;
using OpenAdm.Application.Models;

namespace OpenAdm.Application.Services;

public class ProcessarPedidoService : IProcessarPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IProducerGeneric<ProcessarPedidoModel> _producerGeneric;
    private readonly IPdfPedidoService _pdfPedidoService;
    private readonly IEmailService _emailService;

    public ProcessarPedidoService(
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IProducerGeneric<ProcessarPedidoModel> producerGeneric,
        IPedidoRepository pedidoRepository,
        IPdfPedidoService pdfPedidoService,
        IEmailService emailService)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _producerGeneric = producerGeneric;
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
            var pdf = _pdfPedidoService.GeneratePdfPedido(pedido, null, logo);

            var message = $"Que ótima noticia, mais um pedido!\nN. do pedido : {pedido.Numero}";
            var assunto = "Novo pedido";

            var emailModel = new ToEnvioEmailModel()
            {
                Assunto = assunto,
                Email = configuracoesDePedido.EmailDeEnvio,
                Mensagem = message,
                Arquivo = pdf,
                NomeDoArquivo = $"pedido-{pedido.Numero}",
                TipoDoArquivo = "application/pdf"
            };

            await _emailService.SendEmail(emailModel, new FromEnvioEmailModel()
            {
                Email = EmailConfiguracaoModel.Email,
                Porta = EmailConfiguracaoModel.Porta,
                Senha = EmailConfiguracaoModel.Senha,
                Servidor = EmailConfiguracaoModel.Servidor
            });
        }
    }

    public void ProcessarProdutosMaisVendidosAsync(Pedido pedido)
    {
        var processarPedidoModel = new ProcessarPedidoModel()
        {
            EmailEnvio = "",
            Pedido = pedido
        };

        foreach (var item in pedido.ItensPedido)
        {
            item.Pedido = null!;
            item.Produto = null!;
            item.Peso = null;
            item.Tamanho = null;
        }

        _producerGeneric.Publish(processarPedidoModel, "pedido-entregue");
    }
}
