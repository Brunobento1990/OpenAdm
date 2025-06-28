using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Emails;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public class CancelarPedido : ICancelarPedido
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IEmailApiService _emailService;
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;

    public CancelarPedido(
        IPedidoRepository pedidoRepository,
        IEmailApiService emailService,
        IConfiguracoesDePedidoService configuracoesDePedidoService)
    {
        _pedidoRepository = pedidoRepository;
        _emailService = emailService;
        _configuracoesDePedidoService = configuracoesDePedidoService;
    }

    public async Task<bool> CancelarAsync(CancelarPedidoDto cancelarPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(cancelarPedidoDto.PedidoId)
            ?? throw new ExceptionApi("Não foi possível localizar o pedido");
        pedido.Cancelar(cancelarPedidoDto.Motivo);
        await _pedidoRepository.UpdateAsync(pedido);

        var configuracoesDePedido = await _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();

        if (configuracoesDePedido != null)
        {
            var message = $"Pedido cancelado!\nN. do pedido : {pedido.Numero}";
            var assunto = "Cancelamento de pedido";

            if (!string.IsNullOrWhiteSpace(pedido.MotivoCancelamento))
            {
                message += $"\nMotivo: {pedido.MotivoCancelamento}";
            }

            var emailModel = new ToEnvioEmailApiModel()
            {
                Assunto = assunto,
                Email = configuracoesDePedido.EmailDeEnvio,
                Mensagem = message,
                NomeDoArquivo = $"pedido-{pedido.Numero}"
            };

            await _emailService.SendEmailAsync(emailModel, new FromEnvioEmailApiModel()
            {
                Email = EmailConfiguracaoModel.Email,
                Porta = EmailConfiguracaoModel.Porta,
                Senha = EmailConfiguracaoModel.Senha,
                Servidor = EmailConfiguracaoModel.Servidor
            });
        }

        return true;
    }
}
